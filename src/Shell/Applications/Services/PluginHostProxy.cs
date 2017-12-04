using System;
using System.AddIn.Pipeline;
using System.Diagnostics;
using System.IO;
using System.Threading;
using WpfPluginSample.PluginFramework;
using WpfPluginSample.PluginFramework.Internals;

namespace WpfPluginSample.Shell.Applications.Services
{
    internal class PluginHostProxy : DisposableObject
    {
        private readonly int parentProcessId;
        private readonly string instanceName;
        private IPluginLoader pluginLoader;
        
        private PluginHostProxy(PluginInfo pluginInfo, int parentProcessId, string instanceName)
        {
            PluginInfo = pluginInfo;
            this.parentProcessId = parentProcessId;
            this.instanceName = instanceName;
        }
        
        public PluginInfo PluginInfo { get; }

        public object RemoteView { get; private set; }

        public event EventHandler Disposed;

        public static PluginHostProxy LoadPlugin(PluginInfo pluginInfo)
        {
            int parentProcessId = Process.GetCurrentProcess().Id;
            var proxy = new PluginHostProxy(pluginInfo, parentProcessId, "PluginHost." + Guid.NewGuid());
            proxy.StartHost();
            return proxy;
        }

        private void StartHost()
        {
            var processInfo = new ProcessStartInfo
            {
                Arguments = new TypedArgs(parentProcessId, PluginInfo.AssemblyFile, instanceName).ToArgs(),
                CreateNoWindow = true, // Note: The command window might be helpful to search for errors.
                UseShellExecute = false,
                FileName = "PluginHost.exe",
                WorkingDirectory = Path.GetDirectoryName(PluginInfo.AssemblyFile)
            };

            using (var readyEvent = new EventWaitHandle(false, EventResetMode.ManualReset, instanceName + ".Ready"))
            {
                var pluginProcess = Process.Start(processInfo);
                pluginProcess.EnableRaisingEvents = true;
                pluginProcess.Exited += PluginProcessExited;
                if (!readyEvent.WaitOne(3000))
                {
                    throw new InvalidOperationException("Plugin host process not ready.");
                }
            }

            var url = "ipc://" + instanceName + "/PluginLoader";
            pluginLoader = (IPluginLoader)Activator.GetObject(typeof(IPluginLoader), url);
            // TODO: This won't work if the plugin crashes during startup!
            var contract = pluginLoader.LoadPlugin(PluginInfo.AssemblyFile, PluginInfo.PluginControllerName);
            RemoteView = FrameworkElementAdapters.ContractToViewAdapter(contract);
        }

        private void PluginProcessExited(object sender, EventArgs e)
        {
            Dispose();
        }

        protected override void DisposeCore(bool isDisposing)
        {
            if (isDisposing)
            {
                var loader = pluginLoader;
                pluginLoader = null;
                loader?.Shutdown();
            }
            base.DisposeCore(isDisposing);
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
