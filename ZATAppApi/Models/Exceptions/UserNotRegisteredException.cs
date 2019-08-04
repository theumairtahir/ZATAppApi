using System;

namespace ZATAppApi.Models.Exceptions
{
    /// <summary>
    /// Exception will be thrown when the addition of the user to ASP Identity failed
    /// </summary>
    public sealed class UserNotRegisteredException:Exception
    {
        public UserNotRegisteredException() : base("User not created to the ASP.Net Identity.")
        {

        }
    }
}