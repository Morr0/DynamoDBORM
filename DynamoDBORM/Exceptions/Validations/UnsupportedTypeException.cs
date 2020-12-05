using System;
using DynamoDBORM.Exceptions.Converters;

namespace DynamoDBORM.Exceptions.Validations
{
    public class UnsupportedTypeException : ValidationException
    {
        public UnsupportedTypeException(Type type)
        {
            Type = type;
        }

        public Type Type { get; set; }
    }
}