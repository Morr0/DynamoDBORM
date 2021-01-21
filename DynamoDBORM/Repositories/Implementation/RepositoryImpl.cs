using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Attributes;
using DynamoDBORM.Converters;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Repositories.Implementation
{
    public partial class RepositoryImpl
    {
        private readonly ConversionManager _conversionManager;

        public RepositoryImpl(ConversionManager conversionManager)
        {
            _conversionManager = conversionManager;
        }
        
        private bool Unmapped(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<UnmappedAttribute>() != null;
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
    }
}