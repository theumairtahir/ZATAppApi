using System;

namespace ZATAppApi.Models.Exceptions
{
    /// <summary>
    /// Exception throw whenever the primary key of an entity will be tried to use to access a sibling entity.
    /// </summary>
    public sealed class PrimaryKeyNotForEntityException : Exception
    {
        public PrimaryKeyNotForEntityException(string callingEntity, string sibling):base("You have called a primary key of "+callingEntity+" into "+sibling+" which can't be accessed by this apporach")
        {

        }
    }
}