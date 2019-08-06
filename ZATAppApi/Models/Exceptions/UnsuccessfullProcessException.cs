using System;

namespace ZATApp.Models.Exceptions
{
    /// <summary>
    /// Exception thrown whenever a process don't fulfill as user demand
    /// </summary>
    public sealed class UnsuccessfullProcessException:Exception
    {
        public UnsuccessfullProcessException(string path):base("Process not finished with success. Path: "+path)
        {

        }
    }
}