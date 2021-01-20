using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Exceptions;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Tables
{
    internal static class TableDefinitionUtility
    {
        public static async Task<bool> HasExistingTable(AmazonDynamoDBClient client, TableProfile profile)
        {
            // TODO check types
            try
            {
                var request = new DescribeTableRequest
                {
                    TableName = profile.TableName
                };

                var response = await client.DescribeTableAsync(request).ConfigureAwait(false);
                bool hasSamePartitionKeyName = false;
                bool shouldHaveSortKey = !string.IsNullOrEmpty(profile.SortKeyName);
                bool hasSameSortKeyIfDefined = false;
                
                foreach (var keySchema in response.Table.KeySchema)
                {
                    if (keySchema.AttributeName == profile.PartitionKeyName)
                    {
                        hasSamePartitionKeyName = true;
                    } else if (shouldHaveSortKey && keySchema.AttributeName == profile.SortKeyName)
                    {
                        hasSameSortKeyIfDefined = true;
                    }
                }

                if (!hasSamePartitionKeyName) throw new DynamoDbHasDifferentParitionKeyException();
                if (shouldHaveSortKey && !hasSamePartitionKeyName) throw new DynamoDbHasDifferentSortKeyException();
            }
            catch (TableNotFoundException e)
            {
                return false;
            }

            return true;
        }
    }
}