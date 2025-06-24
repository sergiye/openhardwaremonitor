using System;
using System.IO;
using HidSharp.Utility;

namespace HidSharp
{
    public abstract class DeviceStream : Stream
    {
        /// <summary>
        /// Occurs when the stream is closed.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Occurs when <see cref="OpenOption.Interruptible"/> is <c>true</c> and another process or thread with higher priority
        /// would like to open the stream.
        /// </summary>
        public event EventHandler InterruptRequested;

        /// <exclude/>
        protected DeviceStream(Device device)
        {
            Throw.If.Null(device);
            Device = device;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            try
            {
                OnClosed();
            }
            catch (Exception e)
            {
                HidSharpDiagnostics.Trace("OnClosed threw an exception: {0}", e);
            }

            base.Dispose(disposing);
        }

        /// <inheritdoc/>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            Throw.If.OutOfRange(buffer, offset, count);
            return AsyncResult<int>.BeginOperation(delegate()
            {
                return Read(buffer, offset, count);
            }, callback, state);
        }

        /// <inheritdoc/>
        public override int EndRead(IAsyncResult asyncResult)
        {
            return AsyncResult<int>.EndOperation(asyncResult);
        }

        /// <inheritdoc/>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            Throw.If.OutOfRange(buffer, offset, count);
            return AsyncResult<int>.BeginOperation(delegate()
            {
                Write(buffer, offset, count); return 0;
            }, callback, state);
        }

        /// <inheritdoc/>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            AsyncResult<int>.EndOperation(asyncResult);
        }

        /// <exclude />
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <exclude />
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        protected virtual void OnClosed()
        {
            RaiseClosed();
        }

        protected void RaiseClosed()
        {
            var ev = Closed;
            if (ev != null) { ev(this, EventArgs.Empty); }
        }

        protected internal virtual void OnInterruptRequested()
        {
            RaiseInterruptRequested();
        }

        protected void RaiseInterruptRequested()
        {
            var ev = InterruptRequested;
            if (ev != null) { ev(this, EventArgs.Empty); }
        }

        /// <exclude />
        public override bool CanRead
        {
            get { return true; }
        }

        /// <exclude />
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <exclude />
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <exclude />
        public override bool CanTimeout
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the <see cref="Device"/> associated with this stream.
        /// </summary>
        public Device Device
        {
            get;
            private set;
        }

        /// <exclude />
        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        /// <exclude />
        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The maximum amount of time, in milliseconds, to wait for the device to send some data.
        /// 
        /// The default is 3000 milliseconds.
        /// To disable the timeout, set this to <see cref="System.Threading.Timeout.Infinite"/>.
        /// </summary>
        public abstract override int ReadTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// The maximum amount of time, in milliseconds, to wait for the device to receive the data.
        /// 
        /// The default is 3000 milliseconds.
        /// To disable the timeout, set this to <see cref="System.Threading.Timeout.Infinite"/>.
        /// </summary>
        public abstract override int WriteTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// An object storing user-defined data about the stream.
        /// </summary>
        public object Tag
        {
            get;
            set;
        }
    }
}
