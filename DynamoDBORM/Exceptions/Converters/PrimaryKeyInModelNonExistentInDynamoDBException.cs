namespace DynamoDBORM.Exceptions.Converters
{
    public class PrimaryKeyInModelNonExistentInDynamoDBException : ConversionException
    {
        public PrimaryKeyInModelNonExistentInDynamoDBException(ConversionExceptionReason reason)
        {
            Reason = reason;
        }
    }
}