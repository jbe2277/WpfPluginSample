using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Windows;
using WpfPluginSample.PluginFramework;
using WpfPluginSample.PluginFramework.Internals;

namespace WpfPluginSample.PluginHost
{
    internal class PluginLoader : RemoteService, IPluginLoader
    {
        private IPluginController pluginController;

        public INativeHandleContract LoadPlugin(string assembly, string typeName)
        {
            return (INativeHandleContract)Bootstrapper.Dispatcher.Invoke((Func<string, string, INativeHandleContract>)LoadPluginCore, assembly, typeName);
        }

        private INativeHandleContract LoadPluginCore(string assembly, string typeName)
        {
            var objectHandle = Activator.CreateInstance(assembly, typeName);
            pluginController = (IPluginController)objectHandle.Unwrap();
            pluginController.Initialize();
            var view = pluginController.CreateMainView();
            var contract = FrameworkElementAdapters.ViewToContractAdapter((FrameworkElement)view);
            var insulator = new NativeHandleContractInsulator(contract);
            return insulator;
        }

        public void Shutdown()
        {
            Bootstrapper.Dispatcher.Invoke(ShutdownCore);
            Environment.Exit(0);
        }

        private void ShutdownCore()
        {
            pluginController.Shutdown();
        }
    }
}
