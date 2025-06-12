using System;

namespace HidSharp
{
    static class CommonException
    {
        public static ObjectDisposedException CreateClosedException()
        {
            return new ObjectDisposedException("Closed.", (Exception)null);
        }
    }
}
