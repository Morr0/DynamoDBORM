using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Repositories.Implementation
{
    public partial class RepositoryImpl
    {
        internal async Task<Dictionary<string, AttributeValue>> Get<T>(AmazonDynamoDBClient client
            , TableProfile profile,
            object partitionKeyValue, object sortKeyValue)
        {
            var request = new GetItemRequest
            {
                TableName = profile.TableName,
                Key = Key(profile.PartitionKeyName, profile.SortKeyName, 
                    partitionKeyValue, sortKeyValue)
            };

            return (await client.GetItemAsync(request, CancellationToken.None).ConfigureAwait(false)).Item;
        }
        
        internal async Task<List<Dictionary<string, AttributeValue>>> GetMany<T>(AmazonDynamoDBClient client, TableProfile profile) where T : new()
        {
            var request = new ScanRequest
            {
                TableName = profile.TableName
            };

            return (await client.ScanAsync(request, CancellationToken.None).ConfigureAwait(false)).Items;
        }
        
        internal async Task<TProperty> GetProperty<TProperty>(AmazonDynamoDBClient client, 
            TableProfile profile, object partitionKey, object sortKey, string memberName)
        {
            string dynamoDbName = profile.PropNameToDynamoDbName.ContainsKey(memberName) ?
                profile.PropNameToDynamoDbName[memberName] : memberName;
            
            var request = new GetItemRequest
            {
                TableName = profile.TableName,
                Key = Key(profile.PartitionKeyName, profile.SortKeyName, partitionKey, sortKey),
                ProjectionExpression = dynamoDbName
            };

            var response = await client.GetItemAsync(request).ConfigureAwait(false);
            if (!response.Item.ContainsKey(dynamoDbName)) return default;

            TProperty value = (TProperty) _conversionManager.FromAttVal[typeof(TProperty)]
                (response.Item[dynamoDbName]);
            return value;
        }
    }
}