using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Repositories.Implementation
{
    public partial class RepositoryImpl
    {
        internal Task Remove<T>(AmazonDynamoDBClient client, TableProfile profile, object partitionKey, object sortKey) where T : new()
        {
            var request = new DeleteItemRequest
            {
                TableName = profile.TableName,
                Key = Key(profile.PartitionKeyName, profile.SortKeyName, partitionKey, sortKey)
            };

            return client.DeleteItemAsync(request, CancellationToken.None);
        }
    }
}