using System;

namespace WpfPluginSample.PluginFramework
{
    /// <summary>
    /// Base class that implements the IDisposable interface.
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        private volatile bool isDisposed;


        /// <summary>
        /// Indicates, whether the object was disposed.
        /// </summary>
        protected bool IsDisposed => isDisposed;


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Override this method to free, release or reset any resources.
        /// </summary>
        /// <param name="isDisposing">if true then dispose unmanaged and managed resources; otherwise dispose only unmanaged resources.</param>
        protected virtual void DisposeCore(bool isDisposing)
        {
        }

        private void Dispose(bool isDiposing)
        {
            if (isDisposed) return;
            
            isDisposed = true;
            DisposeCore(isDiposing);
        }
    }
}
