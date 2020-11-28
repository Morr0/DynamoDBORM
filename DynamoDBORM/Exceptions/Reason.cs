namespace DynamoDBORM.Exceptions
{
    public enum Reason : byte
    {
        SameAsExceptionName,
        ReferencedNonExistentPartitionKey,
        ReferencedNonExistentSortKey
    }
}