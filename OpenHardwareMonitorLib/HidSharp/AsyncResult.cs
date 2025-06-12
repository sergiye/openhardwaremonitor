using System;
using System.Threading;

namespace HidSharp
{
    sealed class AsyncResult<T> : IAsyncResult
    {
        volatile bool _isCompleted;
        ManualResetEvent _waitHandle;

        AsyncResult(AsyncCallback callback, object state)
        {
            AsyncCallback = callback; AsyncState = state;
        }

        void Complete()
        {
            lock (this)
            {
                if (_isCompleted) { return; } _isCompleted = true;
                if (_waitHandle != null) { _waitHandle.Set(); }
            }

            if (AsyncCallback != null) { AsyncCallback(this); }
        }

        internal delegate T OperationCallback();

        internal static IAsyncResult BeginOperation(OperationCallback operation,
            AsyncCallback callback, object state)
        {
            var ar = new AsyncResult<T>(callback, state);
            ThreadPool.QueueUserWorkItem(delegate(object self)
            {
                try { ar.Result = operation(); }
                catch (Exception e) { ar.Exception = e; }
                ar.Complete();
            }, ar);
            return ar;
        }

        internal T EndOperation()
        {
            while (true)
            {
                if (IsCompleted)
                {
                    if (Exception != null) { throw Exception; }
                    return Result;
                }
                AsyncWaitHandle.WaitOne();
            }
        }

        internal static T EndOperation(IAsyncResult asyncResult)
        {
            Throw.If.Null(asyncResult);
            return ((AsyncResult<T>)asyncResult).EndOperation();
        }

        public AsyncCallback AsyncCallback
        {
            get;
            private set;
        }

        public object AsyncState
        {
            get;
            private set;
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                lock (this)
                {
                    if (_waitHandle == null)
                    {
                        _waitHandle = new ManualResetEvent(_isCompleted);
                    }
                }

                return _waitHandle;
            }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
        }

        Exception Exception
        {
            get;
            set;
        }

        T Result
        {
            get;
            set;
        }
    }
}
