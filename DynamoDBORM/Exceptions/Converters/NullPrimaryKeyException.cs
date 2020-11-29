namespace DynamoDBORM.Exceptions.Converters
{
    public class NullPrimaryKeyException : ConversionException
    {
        public NullPrimaryKeyException(ConversionExceptionReason reason)
        {
            Reason = reason;
        }
    }
}