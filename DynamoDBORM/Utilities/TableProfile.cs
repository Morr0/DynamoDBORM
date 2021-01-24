using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamoDBORMTest")]
namespace DynamoDBORM.Utilities
{
    internal class TableProfile
    {
        public TableProfile(string name, string partitionKeyName, string sortKeyName, 
            Dictionary<string, string> propNameToDynamoDbName)
        {
            TableName = name;
            PartitionKeyName = partitionKeyName;
            SortKeyName = sortKeyName;

            PropNameToDynamoDbName = propNameToDynamoDbName;
            DynamoDbNameToPropName = propNameToDynamoDbName.ToDictionary(x => x.Value, x => x.Key);
        }

        public string TableName { get; }
        public string PartitionKeyName { get; }
        public string SortKeyName { get; }
        
        public Dictionary<string, string> PropNameToDynamoDbName { get; }
        public Dictionary<string, string> DynamoDbNameToPropName { get; }
    }
}