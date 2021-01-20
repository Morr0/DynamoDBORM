using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Attributes;
using DynamoDBORM.Converters;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Repositories
{
    internal class RepositoryImpl
    {
        private readonly ConversionManager _conversionManager;

        public RepositoryImpl(ConversionManager conversionManager)
        {
            _conversionManager = conversionManager;
        }

        public async Task<Dictionary<string, AttributeValue>> Get<T>(AmazonDynamoDBClient client
            , TableProfile profile,
            object partitionKeyValue, object sortKeyValue)
        {
            var request = new GetItemRequest
            {
                TableName = profile.TableName,
                Key = Key(profile.PartitionKeyName, profile.SortKeyName, 
                    ref partitionKeyValue, ref sortKeyValue)
            };

            return (await client.GetItemAsync(request, CancellationToken.None).ConfigureAwait(false)).Item;
        }

        private Dictionary<string, AttributeValue> Key(string partitionKeyName, string sortKeyName, 
            ref object partitionKey, ref object sortKey)
        {
            var dict = new Dictionary<string, AttributeValue>();

            dict.Add(partitionKeyName, _conversionManager.ToAttVal[partitionKey.GetType()](partitionKey));
            if (!string.IsNullOrEmpty(sortKeyName))
                dict.Add(sortKeyName, _conversionManager.ToAttVal[sortKey.GetType()](sortKey));
            
            return dict;
        }

        public async Task<List<Dictionary<string, AttributeValue>>> GetMany<T>(AmazonDynamoDBClient client, TableProfile profile) where T : new()
        {
            var request = new ScanRequest
            {
                TableName = profile.TableName
            };

            return (await client.ScanAsync(request, CancellationToken.None).ConfigureAwait(false)).Items;
        }

        public Task Add<T>(AmazonDynamoDBClient client, TableProfile profile, T obj) where T : new()
        {
            var request = new PutItemRequest
            {
                TableName = profile.TableName,
                Item = _conversionManager.To(obj)
            };

            return client.PutItemAsync(request, CancellationToken.None);
        }

        public Task Remove<T>(AmazonDynamoDBClient client, TableProfile profile, object partitionKey, object sortKey) where T : new()
        {
            var request = new DeleteItemRequest
            {
                TableName = profile.TableName,
                Key = Key(profile.PartitionKeyName, profile.SortKeyName, ref partitionKey, ref sortKey)
            };

            return client.DeleteItemAsync(request, CancellationToken.None);
        }

        public Task Update<T>(AmazonDynamoDBClient client, TableProfile profile, T obj) where T : new()
        {
            var updatables = GetUpdateExpression<T>(profile);
            var request = new UpdateItemRequest
            {
                TableName = profile.TableName,
                Key = Key<T>(profile.PartitionKeyName, profile.SortKeyName, ref obj),
                UpdateExpression = updatables.updateString,
                ExpressionAttributeValues = updatables.values
            };

            return client.UpdateItemAsync(request, CancellationToken.None);
        }

        private (string updateString, Dictionary<string, AttributeValue> values) GetUpdateExpression<T>(TableProfile profile)
        {
            var props = typeof(T).GetProperties();
            var sb = new StringBuilder(props.Length * 2);
            sb.Append("SET "); // Check AWS docs for updating an item in DynamoDB
            var values = new Dictionary<string, AttributeValue>(props.Length * 2);

            foreach (var prop in props)
            {
                if (Unmapped(prop)) continue;
                
                if (prop.Name != profile.PartitionKeyName && prop.Name != profile.SortKeyName)
                {
                    string valueName = $":{prop.Name}";
                    values.Add(valueName, _conversionManager.To(prop.PropertyType)[prop.Name]);

                    sb.Append($"{prop.Name} = {valueName},");
                }
            }

            sb.Remove(sb.Length - 1, 1);
            string updateString = sb.ToString();
            return (updateString, values);
        }

        private bool Unmapped(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<UnmappedAttribute>() != null;
        }

        private Dictionary<string, AttributeValue> Key<T>(string partitionKeyName, string sortKeyName, ref T obj)
        {
            var type = typeof(T);
            object partitionKeyValue = type.GetProperty(partitionKeyName);
            object sortKeyValue = type.GetProperty(sortKeyName);

            return Key(partitionKeyName, sortKeyName, ref partitionKeyValue, ref sortKeyValue);
        }
    }
}