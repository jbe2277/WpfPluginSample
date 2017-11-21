using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

namespace WpfPluginSample.PluginFramework
{
    public static class RemoteServiceLocator
    {
        private static readonly object initializeIpcChannelLock = new object();


        public static TInterface GetService<TInterface>()
            where TInterface : class
        {
            string objectUri = "ipc://" + GetIpcPortName<TInterface>() + "/" + typeof(TInterface).FullName;
            return (TInterface)Activator.GetObject(typeof(TInterface), objectUri);
        }
        
        public static string GetIpcPortName<TInterface>()
            where TInterface : class
        {
            // TODO: Optional use the GUID attribute or introduce an own assembly attribute.
            return typeof(TInterface).Assembly.GetName().Name;
        }

        public static void RegisterType<TInterface, TImplementation>() 
            where TInterface : class
            where TImplementation : MarshalByRefObject, TInterface
        {
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(TImplementation), typeof(TInterface).FullName, WellKnownObjectMode.Singleton);
        }

        public static void RegisterInstance<TInterface>(MarshalByRefObject instance)
            where TInterface : class
        {
            RemotingServices.Marshal(instance, typeof(TInterface).FullName);
        }

        // Call this only once for a process
        // ipcPortName is necessary if process provides services
        public static IpcChannel InitializeIpcChannel(string ipcPortName = null)
        {
            lock (initializeIpcChannelLock)
            {
                IpcChannel channel;
                if (string.IsNullOrEmpty(ipcPortName))
                {
                    // Only a client channel
                    channel = new IpcChannel();
                }
                else
                {
                    // Channel that supports server and client usage
                    var serverProvider = new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full };
                    var clientProvider = new BinaryClientFormatterSinkProvider();
                    var properties = new Dictionary<string, string>();
                    properties["portName"] = ipcPortName;
                    channel = new IpcChannel(properties, clientProvider, serverProvider);
                }

                ChannelServices.RegisterChannel(channel, false);
                return channel;
            }
        }
    }
}
