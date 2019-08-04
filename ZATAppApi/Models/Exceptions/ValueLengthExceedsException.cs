using System;

namespace ZATAppApi.Models.Exceptions
{
    /// <summary>
    /// Exception thrown whenever a value enter to the string exceeds to the defined limit
    /// </summary>
    public sealed class ValueLengthExceedsException:Exception
    {
        /// <summary>
        /// Contructor which initialzes the message
        /// </summary>
        /// <param name="value">Entered by the user</param>
        /// <param name="definedLength">Defined Length of the string</param>
        public ValueLengthExceedsException(string value, int definedLength):base("The entered value: "+value+" is greater then the defined limit: " + definedLength)
        {

        }
    }
}