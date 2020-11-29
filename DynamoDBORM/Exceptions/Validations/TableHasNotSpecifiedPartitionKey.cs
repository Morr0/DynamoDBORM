namespace DynamoDBORM.Exceptions.Validations
{
    public class TableHasNotSpecifiedPartitionKey : ValidationException
    {
        public TableHasNotSpecifiedPartitionKey(Reason reason = Reason.SameAsExceptionName)
        {
            Reason = reason;
        }
    }
}