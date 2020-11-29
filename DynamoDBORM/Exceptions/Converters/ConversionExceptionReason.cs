namespace DynamoDBORM.Exceptions.Converters
{
    public enum ConversionExceptionReason : byte
    {
        Unspecified,
        NullPartitionKey,
        NullSortKey,
        NonExistentPartitionKey,
        NonExistentSortKey
    }
}