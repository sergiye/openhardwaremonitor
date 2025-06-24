namespace HidSharp.Platform
{
    abstract class SysBleStream : Experimental.BleStream
    {
        internal SysBleStream(Experimental.BleDevice device, Experimental.BleService service)
            : base(device, service)
        {

        }

        #region Reference Counting
        SysRefCountHelper _rch;

        internal void HandleInitAndOpen()
        {
            _rch.HandleInitAndOpen();
        }

        internal bool HandleClose()
        {
            return _rch.HandleClose();
        }

        internal bool HandleAcquire()
        {
            return _rch.HandleAcquire();
        }

        internal void HandleAcquireIfOpenOrFail()
        {
            _rch.HandleAcquireIfOpenOrFail();
        }

        internal void HandleRelease()
        {
            if (_rch.HandleRelease()) { HandleFree(); }
        }

        internal abstract void HandleFree();
        #endregion
    }
}
