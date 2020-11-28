using System;

namespace DynamoDBORM.Exceptions
{
    public class ValidationException : Exception
    {
        public virtual Reason Reason { get; set; }
    }
}