using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZATAppApi.Models.Exceptions
{
    /// <summary>
    /// Exception thrown whenever some unwanted values are found
    /// </summary>
    public sealed class MalValueArrivedException:Exception
    {
        public MalValueArrivedException(string path):base("Some unwanted value captured by the system. Path: " + path)
        {

        }
    }
}