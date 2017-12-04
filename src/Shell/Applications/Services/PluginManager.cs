using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Waf.Applications;
using WpfPluginSample.PluginFramework;

namespace WpfPluginSample.Shell.Applications.Services
{
    public class PluginManager : DisposableObject
    {
        private readonly List<PluginHostProxy> pluginHostProxies;
        
        public PluginManager()
        {
            pluginHostProxies = new List<PluginHostProxy>();
        }

        public event EventHandler<PluginUnloadedEventArgs> PluginUnloaded;

        public static async Task<IReadOnlyList<PluginInfo>> DiscoverAsync()
        {
            var plugingPath = Path.Combine(ApplicationInfo.ApplicationPath, "Plugins");
            var dllFiles = Directory.GetFiles(plugingPath, "*.dll", SearchOption.AllDirectories);
            var result = await Task.WhenAll(dllFiles.Select(x => Task.Run(() => PluginMetadataReader.Read(x)))).ConfigureAwait(false);
            return result.Where(x => x != null).ToArray();
        }

        public object Load(PluginInfo pluginInfo)
        {
            var pluginHostProxy = PluginHostProxy.LoadPlugin(pluginInfo);
            pluginHostProxy.Disposed += PluginHostProxyDisposed;
            pluginHostProxies.Add(pluginHostProxy);
            return pluginHostProxy.RemoteView;
        }
        
        public void Unload(object pluginView)
        {
            var pluginHostProxy = pluginHostProxies.Single(x => x.RemoteView == pluginView);
            pluginHostProxy.Dispose();
            pluginHostProxies.Remove(pluginHostProxy);
        }

        private void PluginHostProxyDisposed(object sender, EventArgs e)
        {
            PluginUnloaded?.Invoke(this, new PluginUnloadedEventArgs(((PluginHostProxy)sender).RemoteView));
        }

        protected override void DisposeCore(bool isDisposing)
        {
            if (isDisposing)
            {
                var proxies = pluginHostProxies.ToList();
                pluginHostProxies.Clear();
                proxies.ForEach(x => x.Dispose());
            }
            base.DisposeCore(isDisposing);
        }
    }

    public sealed class PluginUnloadedEventArgs
    {
        public PluginUnloadedEventArgs(object pluginView)
        {
            PluginView = pluginView;
        }

        public object PluginView { get; }
    }
}
