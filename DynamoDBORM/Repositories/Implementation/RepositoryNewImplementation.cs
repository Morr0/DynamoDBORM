using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Repositories.Implementation
{
    public partial class RepositoryImpl
    {
        internal Task Add<T>(AmazonDynamoDBClient client, TableProfile profile, T obj) where T : new()
        {
            var request = new PutItemRequest
            {
                TableName = profile.TableName,
                Item = _conversionManager.To(obj)
            };

            return client.PutItemAsync(request, CancellationToken.None);
        }
    }
}