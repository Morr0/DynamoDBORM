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
                    partitionKeyValue, sortKeyValue)
            };

            return (await client.GetItemAsync(request, CancellationToken.None).ConfigureAwait(false)).Item;
        }

        private Dictionary<string, AttributeValue> Key(string partitionKeyName, string sortKeyName, 
            object partitionKey, object sortKey)
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
                Key = Key(profile.PartitionKeyName, profile.SortKeyName, partitionKey, sortKey)
            };

            return client.DeleteItemAsync(request, CancellationToken.None);
        }

        public Task Update<T>(AmazonDynamoDBClient client, TableProfile profile, T obj) where T : new()
        {
            var props = typeof(T).GetProperties();
            var sb = new StringBuilder(props.Length + 1).Append("SET ");
            var values = new Dictionary<string, AttributeValue>(props.Length);

            foreach (var prop in props)
            {
                if (Unmapped(prop)) continue;
                if (prop.Name == profile.PartitionKeyName || prop.Name == profile.SortKeyName) continue;
                var propValue = prop.GetValue(obj);

                string valueName = $":{prop.Name}";
                values.Add(valueName, _conversionManager.ToAttVal[prop.PropertyType](propValue));
                sb.Append($"{prop.Name}={valueName},");
            }
            string updateString = sb.ToString().TrimEnd(',');
            
            var request = new UpdateItemRequest
            {
                TableName = profile.TableName,
                Key = Key<T>(profile, obj),
                UpdateExpression = updateString,
                ExpressionAttributeValues = values
            };

            return client.UpdateItemAsync(request, CancellationToken.None);
        }

        private bool Unmapped(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<UnmappedAttribute>() != null;
        }

        private Dictionary<string, AttributeValue> Key<T>(TableProfile profile, T obj)
        {
            var type = typeof(T);
            object partitionKeyValue = type.GetProperty(profile.PartitionKeyName)?.GetValue(obj);
            object sortKeyValue = null;
            if (profile.SortKeyName is null) return Key(profile.PartitionKeyName, null, 
                    partitionKeyValue, sortKeyValue);
            
            sortKeyValue = type.GetProperty(profile.SortKeyName)?.GetValue(obj);
            return Key(profile.PartitionKeyName, profile.SortKeyName, partitionKeyValue, sortKeyValue);
        }

        public async Task<TProperty> GetProperty<TProperty>(AmazonDynamoDBClient client, 
            TableProfile profile, object partitionKey, object sortKey, string memberName)
        {
            var request = new GetItemRequest
            {
                TableName = profile.TableName,
                Key = Key(profile.PartitionKeyName, profile.SortKeyName, partitionKey, sortKey),
                ProjectionExpression = memberName
            };

            var response = await client.GetItemAsync(request).ConfigureAwait(false);
            if (!response.Item.ContainsKey(memberName)) return default;

            TProperty value = (TProperty) _conversionManager.FromAttVal[typeof(TProperty)]
                (response.Item[memberName]);
            return value;
        }
    }
}