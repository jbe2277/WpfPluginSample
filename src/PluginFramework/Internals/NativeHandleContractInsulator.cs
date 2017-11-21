using System;
using System.AddIn.Contract;

namespace WpfPluginSample.PluginFramework.Internals
{
    public class NativeHandleContractInsulator : RemoteService, INativeHandleContract
    {
        private readonly INativeHandleContract source;

        public NativeHandleContractInsulator(INativeHandleContract source)
        {
            this.source = source;
        }

        public IntPtr GetHandle()
        {
            return source.GetHandle();
        }

        public int AcquireLifetimeToken()
        {
            return source.AcquireLifetimeToken();
        }

        public int GetRemoteHashCode()
        {
            return source.GetRemoteHashCode();
        }

        public IContract QueryContract(string contractIdentifier)
        {
            return source.QueryContract(contractIdentifier);
        }

        public bool RemoteEquals(IContract contract)
        {
            return source.RemoteEquals(contract);
        }

        public string RemoteToString()
        {
            return source.RemoteToString();
        }

        public void RevokeLifetimeToken(int token)
        {
            source.RevokeLifetimeToken(token);
        }
    }
}
