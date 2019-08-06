using System;
using System.Collections.Generic;

namespace ZATApp.Models.Exceptions
{
    /// <summary>
    /// Exception will be thrown when the addition of the user to ASP Identity failed
    /// </summary>
    public sealed class UserNotRegisteredException : Exception
    {
        string message = "User not created to the ASP.Net Identity. ERRORS: ";
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public UserNotRegisteredException(IEnumerable<string> lstErrorMessages)
        {
            foreach (var item in lstErrorMessages)
            {
                message += "\n" + item;
            }
        }
        public UserNotRegisteredException()
        {

        }
        public override string Message
        {
            get
            {
                return message;
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}