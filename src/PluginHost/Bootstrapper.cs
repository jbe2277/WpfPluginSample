using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Threading;
using WpfPluginSample.PluginFramework;
using WpfPluginSample.PluginFramework.Internals;

namespace WpfPluginSample.PluginHost
{
    public class Bootstrapper : RemoteService
    {
        private static Process parentProcess;

        public static Dispatcher Dispatcher { get; private set; }

        public void RunPluginDomain(string[] args)
        {
            var typedArgs = TypedArgs.FromArgs(args);

            parentProcess = Process.GetProcessById(typedArgs.ParentProcessId);
            parentProcess.Exited += ParentProcessExited;
            parentProcess.EnableRaisingEvents = true;

            Dispatcher = Dispatcher.CurrentDispatcher;

            RemoteServiceLocator.InitializeIpcChannel(typedArgs.InstanceName);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(PluginLoader), "PluginLoader", WellKnownObjectMode.Singleton);

            // Signal ready
            var readyEvent = EventWaitHandle.OpenExisting(typedArgs.InstanceName + ".Ready");
            readyEvent.Set();

            Dispatcher.Run();
        }

        private static void ParentProcessExited(object sender, EventArgs e)
        {
            // This happens only if the parent process crashed.
            Environment.Exit(1);
        }
    }
}
