namespace DynamoDBORM.Exceptions.Converters
{
    public enum ConversionExceptionReason : byte
    {
        NullPartitionKey,
        NullSortKey
    }
}