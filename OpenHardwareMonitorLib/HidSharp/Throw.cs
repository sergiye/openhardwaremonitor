using System;
using System.Collections.Generic;

namespace HidSharp
{
    sealed class Throw
    {
        Throw()
        {

        }

        public static Throw If
        {
            get { return null; }
        }
    }

    static class ThrowExtensions
    {
        public static Throw False(this Throw self, bool condition)
        {
            if (!condition) { throw new ArgumentException(); }
            return null;
        }

        public static Throw False(this Throw self, bool condition, string message, string paramName)
        {
            if (!condition) { throw new ArgumentException(message, paramName); }
            return null;
        }

        public static Throw Negative(this Throw self, int value, string paramName)
        {
            if (value < 0) { throw new ArgumentOutOfRangeException(paramName); }
            return null;
        }

        public static Throw Null<T>(this Throw self, T value)
        {
            if (value == null) { throw new ArgumentNullException(); }
            return null;
        }

        public static Throw Null<T>(this Throw self, T value, string paramName)
        {
            if (value == null) { throw new ArgumentNullException(paramName); }
            return null;
        }

        public static Throw NullOrEmpty(this Throw self, string value, string paramName)
        {
            Throw.If.Null(value, paramName);
            if (value.Length == 0) { throw new ArgumentException("Must not be empty.", paramName); }
            return null;
        }

        public static Throw OutOfRange(this Throw self, int bufferSize, int offset, int count)
        {
            if (offset < 0 || offset > bufferSize) { throw new ArgumentOutOfRangeException("offset"); }
            if (count < 0 || count > bufferSize - offset) { throw new ArgumentOutOfRangeException("count"); }
            return null;
        }

        public static Throw OutOfRange<T>(this Throw self, IList<T> buffer, int offset, int count)
        {
            Throw.If.Null(buffer, "buffer").OutOfRange(buffer.Count, offset, count);
            return null;
        }
    }
}
