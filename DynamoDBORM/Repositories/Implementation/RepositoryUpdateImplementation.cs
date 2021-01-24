using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Repositories.Implementation
{
    public partial class RepositoryImpl
    {
        internal Task Update<T>(AmazonDynamoDBClient client, TableProfile profile, T obj) where T : new()
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
        
        internal async Task<TModel> UpdateProperty<TModel, TProperty>(AmazonDynamoDBClient client, TableProfile profile, 
            object partitionKey, object sortKey, string memberName, TProperty value)
            where TModel : new()
        {
            string attValName = $":{memberName}";
            var request = new UpdateItemRequest
            {
                TableName = profile.TableName,
                Key = Key(profile.PartitionKeyName, profile.SortKeyName, partitionKey, sortKey),
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {attValName, _conversionManager.ToAttVal[typeof(TProperty)](value)}
                },
                UpdateExpression = $"SET {memberName}={attValName}",
                ReturnValues = ReturnValue.ALL_NEW
            };

            var response = await client.UpdateItemAsync(request).ConfigureAwait(false);
            return _conversionManager.From<TModel>(profile, response.Attributes);
        }

        internal async Task AddOffsetToNumberAttribute<TModel>(AmazonDynamoDBClient client, TableProfile profile,
            object partitionKey, object sortKey, string memberName, string offset) where TModel : new()
        {
            string attValName = $":{memberName}";
            var values = new Dictionary<string, AttributeValue>
            {
                { attValName, new AttributeValue {N = offset}}
            };
            string updateExpr = $"ADD {memberName} {attValName}";
            var request = new UpdateItemRequest
            {
                TableName = profile.TableName,
                Key = Key(profile.PartitionKeyName, profile.SortKeyName, partitionKey, sortKey),
                ExpressionAttributeValues = values,
                UpdateExpression = updateExpr,
                ReturnValues = ReturnValue.ALL_NEW
            };

            await client.UpdateItemAsync(request).ConfigureAwait(false);
        }
    }
}