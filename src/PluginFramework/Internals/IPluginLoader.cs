using System.AddIn.Contract;
using System.Runtime.Remoting.Messaging;

namespace WpfPluginSample.PluginFramework.Internals
{
    public interface IPluginLoader
    {
        INativeHandleContract LoadPlugin(string assembly, string typeName);

        [OneWay]
        void Shutdown();
    }
}
