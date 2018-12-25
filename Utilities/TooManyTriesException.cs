using System;

namespace Chinchillada.RandomExtensions.Utilities
{
    /// <summary>
    /// Exception thrown when a a non-deterministic method is has tried too many times with negative results.
    /// </summary>
    internal class TooManyTriesException : Exception
    {
        public TooManyTriesException()
        {
        }

        public TooManyTriesException(string message)
            : base(message)
        {
        }

        public TooManyTriesException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
