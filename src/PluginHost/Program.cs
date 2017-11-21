using System;
using System.IO;
using System.Reflection;
using WpfPluginSample.PluginFramework;
using WpfPluginSample.PluginFramework.Internals;
using WpfPluginSample.Shell.Interfaces;

namespace WpfPluginSample.PluginHost
{
    internal class Program
    {
        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomainHost)]
        private static void Main(string[] args)
        {
            var typedArgs = TypedArgs.FromArgs(args);

            var config = new RuntimeConfig();
            config.AddCodeBaseFor(typeof(Program).Assembly.GetName());
            config.AddCodeBaseFor(typeof(IPluginController).Assembly.GetName());
            config.AddCodeBaseFor(typeof(ILogService).Assembly.GetName());

            var setup = new AppDomainSetup()
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Environment.CurrentDirectory,
                PrivateBinPathProbe = "ExcludeAppBase",
                ConfigurationFile = typedArgs.AssemblyFile + ".config"
            };
            setup.SetConfigurationBytes(config.GetBytes());
            
            var pluginDomain = AppDomain.CreateDomain("Plugin", null, setup);
            pluginDomain.AssemblyResolve += AssemblyResolve;
            
            var bootstrapper = (Bootstrapper)pluginDomain.CreateInstanceAndUnwrap(typeof(Bootstrapper).Assembly.FullName, typeof(Bootstrapper).FullName);
            bootstrapper.RunPluginDomain(args);
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs e)
        {
            try
            {
                if (File.Exists(e.Name))
                {
                    return Assembly.LoadFrom(e.Name);
                }
                // During probing for satellite assemblies it can happen that an assembly does not exists.
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("AssemblyResolve: " + ex);
                return null;
            }
        }
    }
}
