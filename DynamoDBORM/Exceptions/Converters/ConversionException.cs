using System;

namespace DynamoDBORM.Exceptions.Converters
{
    public class ConversionException : Exception
    {
        public ConversionException()
        {
            Reason = ConversionExceptionReason.Unspecified;
        }
        
        public ConversionExceptionReason Reason { get; set; }
    }
}