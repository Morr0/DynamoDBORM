using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Converters;

namespace DynamoDBORM.Repositories
{
    internal class RepositoryImpl
    {
        private readonly ConversionManager _manager;
        private AmazonDynamoDBClient _client;

        public RepositoryImpl(ConversionManager manager, AmazonDynamoDBClient client)
        {
            _manager = manager;
            _client = client;
        }

        public async Task<Dictionary<string, AttributeValue>> Get<T>(TableProfile profile,
            object partitionKeyValue, object sortKeyValue)
        {
            var request = new GetItemRequest
            {
                TableName = profile.TableName,
                Key = Key<T>(profile.PartitionKeyName, profile.SortKeyName, 
                    ref partitionKeyValue, ref sortKeyValue)
            };

            return (await _client.GetItemAsync(request, CancellationToken.None).ConfigureAwait(false)).Item;
        }

        private Dictionary<string, AttributeValue> Key<T>(string partitionKeyName, string sortKeyName, 
            ref object partitionKey, ref object sortKey)
        {
            var dict = new Dictionary<string, AttributeValue>();

            dict.Add(partitionKeyName, _manager.ToAttVal[partitionKey.GetType()](partitionKey));
            if (!string.IsNullOrEmpty(sortKeyName))
                dict.Add(sortKeyName, _manager.ToAttVal[sortKey.GetType()](sortKey));
            
            return dict;
        }

        public async Task<List<Dictionary<string, AttributeValue>>> GetMany<T>(TableProfile profile) where T : new()
        {
            var request = new ScanRequest
            {
                TableName = profile.TableName
            };

            return (await _client.ScanAsync(request, CancellationToken.None).ConfigureAwait(false)).Items;
        }

        public async Task Add<T>(TableProfile profile, T obj) where T : new()
        {
            var request = new PutItemRequest
            {
                TableName = profile.TableName,
                Item = _manager.To(obj)
            };
        }

        public Task Remove<T>(TableProfile profile, object partitionKey, object sortKey) where T : new()
        {
            var request = new DeleteItemRequest
            {
                TableName = profile.TableName,
                Key = Key<T>(profile.PartitionKeyName, profile.SortKeyName, ref partitionKey, ref sortKey)
            };

            return _client.DeleteItemAsync(request, CancellationToken.None);
        }
    }
}