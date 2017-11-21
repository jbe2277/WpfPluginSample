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
        
        public async Task<IReadOnlyList<PluginInfo>> DiscoverAsync()
        {
            var plugingPath = Path.Combine(ApplicationInfo.ApplicationPath, "Plugins");
            var dllFiles = Directory.GetFiles(plugingPath, "*.dll", SearchOption.AllDirectories);
            var result = await Task.WhenAll(dllFiles.Select(x => Task.Run(() => PluginMetadataReader.Read(x)))).ConfigureAwait(false);
            return result.Where(x => x != null).ToArray();
        }

        public object Load(PluginInfo pluginInfo)
        {
            var pluginHostProxy = PluginHostProxy.LoadPlugin(pluginInfo);
            pluginHostProxies.Add(pluginHostProxy);
            return pluginHostProxy.RemoteView;
        }

        public void Unload(PluginInfo pluginInfo)
        {
            var pluginHostProxy = pluginHostProxies.FirstOrDefault(x => x.PluginInfo == pluginInfo);
            if (pluginHostProxy != null)
            {
                pluginHostProxy.UnloadPlugin();
                pluginHostProxies.Remove(pluginHostProxy);
            }
        }

        protected override void DisposeCore(bool isDisposing)
        {
            if (isDisposing)
            {
                var proxies = pluginHostProxies.ToList();
                pluginHostProxies.Clear();
                proxies.ForEach(x => x.UnloadPlugin());
            }
            base.DisposeCore(isDisposing);
        }
    }
}
