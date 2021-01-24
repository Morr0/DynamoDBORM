using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Attributes;
using DynamoDBORM.Exceptions.Converters;
using DynamoDBORM.Exceptions.Validations;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Converters.Internals
{
    internal class ConvertToImplementation
    {
        private ConversionManager _manager;

        public ConvertToImplementation(ConversionManager manager)
        {
            _manager = manager;
        }
        
        public Dictionary<string, AttributeValue> To<T>(TableProfile profile, T table)
        {
            var dict = new Dictionary<string, AttributeValue>();
            var type = table.GetType();

            EnsurePrimaryKeyIsNotNull(ref type, profile, table);
            EnsureIndexesNotNull(ref type, profile, table);

            foreach (var pair in profile.PropNameToDynamoDbName)
            {
                string propName = pair.Key;
                string dynamoDbName = pair.Value;

                dict.Add(dynamoDbName, GetAttribute(type.GetProperty(propName), table));
            }

            // foreach (var prop in typeof(T).GetProperties())
            // {
            //     if (prop.GetCustomAttribute<UnmappedAttribute>() is not null) continue;
            //     
            //     string propName = prop.Name;
            //     var partitionKeyAttribute = prop.GetCustomAttribute<PartitionKeyAttribute>();
            //     var sortKeyAttribute = prop.GetCustomAttribute<SortKeyAttribute>();
            //     var attributeNameAttribute = prop.GetCustomAttribute<AttributeNameAttribute>();
            //
            //     if (partitionKeyAttribute is not null)
            //     {
            //         propName = !string.IsNullOrEmpty(partitionKeyAttribute.Name) ? partitionKeyAttribute.Name : propName;
            //         if (prop.GetValue(table) is null) throw new NullPartitionKeyException();
            //     } else if (sortKeyAttribute is not null)
            //     {
            //         propName = !string.IsNullOrEmpty(sortKeyAttribute.Name) ? sortKeyAttribute.Name : propName;
            //         if (prop.GetValue(table) is null) throw new NullSortKeyException();
            //     }
            //     else if (attributeNameAttribute is not null)
            //     {
            //         propName = !string.IsNullOrEmpty(attributeNameAttribute.Name)
            //             ? attributeNameAttribute.Name
            //             : propName;
            //     }
            //
            //     dict.Add(propName, GetAttribute(prop, table));
            // }

            return dict;
        }

        private void EnsureIndexesNotNull<T>(ref Type type, TableProfile profile, T table)
        {
            // TODO implement LSI and learn from the other method
        }

        private void EnsurePrimaryKeyIsNotNull<T>(ref Type type, TableProfile profile, T table)
        {
            string propPartitionName = profile.DynamoDbNameToPropName[profile.PartitionKeyName];
            
            if (type.GetProperty(propPartitionName)?.GetValue(table) is null)
                throw new NullPartitionKeyException();
            if (!string.IsNullOrEmpty(profile.SortKeyName) &&
                type.GetProperty(profile.DynamoDbNameToPropName[profile.SortKeyName])
                    ?.GetValue(table) is null)
                throw new NullSortKeyException();
        }

        private AttributeValue GetAttribute<T>(PropertyInfo prop, T table)
        {
            if (!_manager.ToAttVal.ContainsKey(prop.PropertyType))
                throw new UnsupportedTypeException(prop.PropertyType);
            
            return _manager.ToAttVal[prop.PropertyType](prop.GetValue(table));
        }
    }
}