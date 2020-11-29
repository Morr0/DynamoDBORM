using System;

namespace DynamoDBORM.Exceptions.Validations
{
    public class ValidationException : Exception
    {
        public virtual Reason Reason { get; set; }
    }
}