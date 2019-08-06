using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZATApp.Models.Exceptions
{
    /// <summary>
    /// Exception thrown whenever a string fails a certain format
    /// </summary>
    public class InvalidFormatException : Exception
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public InvalidFormatException(string format, string paramName) : base("Your string: " + paramName + " should follow the format: " + format)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {

        }
    }
}