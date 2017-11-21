using System;
using System.Security;

namespace WpfPluginSample.PluginFramework.Internals
{
    /// <summary>
    /// Base class for singleton services which can be accessed over application domain boundaries via remoting.
    /// </summary>
    [Serializable]
    public abstract class RemoteService : MarshalByRefObject
    {
        [SecurityCritical]
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
