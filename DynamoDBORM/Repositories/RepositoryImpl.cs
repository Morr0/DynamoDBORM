using System.Collections.Generic;
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

            return (await _client.GetItemAsync(request).ConfigureAwait(false)).Item;
        }

        private Dictionary<string, AttributeValue> Key<T>(string partitionKeyName, string sortKeyName, 
            ref object partitionKey, ref object sortKey)
        {
            var dict = new Dictionary<string, AttributeValue>();
            var type = typeof(T);
            
            dict.Add(partitionKeyName, _manager.ToAttVal[type](partitionKey));
            if (!string.IsNullOrEmpty(partitionKeyName))
                dict.Add(sortKeyName, _manager.ToAttVal[type](sortKey));
            
            return dict;
        }

        public async Task<List<Dictionary<string, AttributeValue>>> GetMany<T>(TableProfile profile) where T : new()
        {
            var request = new ScanRequest
            {
                TableName = profile.TableName
            };

            return (await _client.ScanAsync(request).ConfigureAwait(false)).Items;
        }
    }
}