using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZATAppApi.Models.Exceptions
{
    /// <summary>
    /// Exception thrown if the value of a veriable is not allowed to change its value
    /// </summary>
    public class NotAuthorizedToChangeValueExeption : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="variableName"></param>
        public NotAuthorizedToChangeValueExeption(string path, string variableName) : base("The variable " + variableName + " is not allowed to chnage its value. Path: " + path)
        {

        }
    }
}