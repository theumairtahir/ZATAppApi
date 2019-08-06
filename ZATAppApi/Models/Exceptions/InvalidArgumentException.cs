using System;

namespace ZATApp.Models.Exceptions
{
    /// <summary>
    /// Exception thrown whenever an unsupported value is captured by a method
    /// </summary>
    public class InvalidArgumentException : Exception
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public InvalidArgumentException(string message) : base(message)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {

        }
    }
}