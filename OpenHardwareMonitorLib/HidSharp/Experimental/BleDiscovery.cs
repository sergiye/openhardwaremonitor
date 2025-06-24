using System;

namespace HidSharp.Experimental
{
    abstract class BleDiscovery : IDisposable
    {
        public abstract void StopDiscovery();

        void IDisposable.Dispose()
        {
            StopDiscovery();
        }
    }
}
