using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZATApp.Models.Exceptions
{
    /// <summary>
    /// This exception will be thrown whenever user violates the unique key constraint for the SQL data
    /// </summary>
    public class UniqueKeyViolationException : Exception
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public UniqueKeyViolationException(string message) : base(message)
        {

        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}