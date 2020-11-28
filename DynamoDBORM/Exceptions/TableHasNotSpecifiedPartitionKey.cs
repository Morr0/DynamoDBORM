namespace DynamoDBORM.Exceptions
{
    public class TableHasNotSpecifiedPartitionKey : ValidationException
    {
        public TableHasNotSpecifiedPartitionKey(Reason reason = Reason.SameAsExceptionName)
        {
            Reason = reason;
        }
    }
}