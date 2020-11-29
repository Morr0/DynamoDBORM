using System;

namespace DynamoDBORM.Exceptions.Converters
{
    public class UnsupportedTypeException : ConversionException
    {
        public UnsupportedTypeException(Type type)
        {
            Type = type;
        }

        public Type Type { get; set; }
    }
}