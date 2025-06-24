using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace HidSharp.Platform
{
    abstract class SysHidStream : HidStream
    {
        protected SysHidStream(HidDevice device)
            : base(device)
        {

        }

        internal class CommonOutputReport
        {
            public byte[] Bytes;
            public bool DoneOK, Feature;
            public volatile bool Done;
        }

        internal static int GetTimeout(int startTime, int timeout)
        {
            return Math.Min(timeout, Math.Max(0, startTime + timeout - Environment.TickCount));
        }

        internal void CommonDisconnected(Queue<byte[]> readQueue)
        {
            lock (readQueue)
            {
                if (readQueue.Count == 0 || null != readQueue.Peek()) { readQueue.Enqueue(null); Monitor.PulseAll(readQueue); }
            }
        }

        internal int CommonRead(byte[] buffer, int offset, int count, Queue<byte[]> queue)
        {
            Throw.If.OutOfRange(buffer, offset, count);
            if (count == 0) { return 0; }

            int readTimeout = ReadTimeout;
            int startTime = Environment.TickCount;
            int timeout;

            HandleAcquireIfOpenOrFail();
            try
            {
                lock (queue)
                {
                    while (true)
                    {
                        if (queue.Count > 0)
                        {
                            if (null == queue.Peek()) { throw new IOException("I/O disconnected."); } // Disconnected.

                            byte[] packet = queue.Dequeue();
                            count = Math.Min(count, packet.Length);
                            Array.Copy(packet, 0, buffer, offset, count);
                            return count;
                        }

                        timeout = GetTimeout(startTime, readTimeout);
                        _rch.ThrowIfClosed();
                        if (!Monitor.Wait(queue, timeout)) { throw new TimeoutException(); }
                    }
                }
            }
            finally
            {
                HandleRelease();
            }
        }

        internal void CommonWrite(byte[] buffer, int offset, int count,
                                  Queue<CommonOutputReport> queue,
                                  bool feature, int maxOutputReportLength)
        {
            Throw.If.OutOfRange(buffer, offset, count);
            count = Math.Min(count, maxOutputReportLength);
            if (count == 0) { return; }

            int writeTimeout = WriteTimeout;
            int startTime = Environment.TickCount;
            int timeout;

            HandleAcquireIfOpenOrFail();
            try
            {
                lock (queue)
                {
                    while (true)
                    {
                        if (queue.Count == 0)
                        {
                            byte[] packet = new byte[count];
                            Array.Copy(buffer, offset, packet, 0, count);
                            var outputReport = new CommonOutputReport() { Bytes = packet, Feature = feature };
                            queue.Enqueue(outputReport);
                            Monitor.PulseAll(queue);

                            while (true)
                            {
                                if (outputReport.Done)
                                {
                                    if (!outputReport.DoneOK) { throw new IOException("I/O output report failed."); }
                                    return;
                                }

                                timeout = GetTimeout(startTime, writeTimeout);
                                _rch.ThrowIfClosed();
                                if (!Monitor.Wait(queue, timeout)) { throw new TimeoutException(); }
                            }
                        }
                        else
                        {
                            timeout = GetTimeout(startTime, writeTimeout);
                            _rch.ThrowIfClosed();
                            if (!Monitor.Wait(queue, timeout)) { throw new TimeoutException(); }
                        }
                    }
                }
            }
            finally
            {
                HandleRelease();
            }
        }

        public sealed override int ReadTimeout
        {
            get;
            set;
        }

        public sealed override int WriteTimeout
        {
            get;
            set;
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
