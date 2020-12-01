using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamoDBORMTest")]
namespace DynamoDBORM.Repositories
{
    internal class TableProfile
    {
        public TableProfile(string name, string partitionKeyName, string sortKeyName)
        {
            TableName = name;
            PartitionKeyName = partitionKeyName;
            SortKeyName = sortKeyName;
        }
        
        public string TableName { get; }
        public string PartitionKeyName { get; }
        public string SortKeyName { get; }
    }
}