﻿using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Attributes;
using DynamoDBORM.Exceptions.Converters;
using DynamoDBORM.Exceptions.Validations;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Converters.Internals
{
    internal class ConvertFromImplementation
    {
        private ConversionManager _manager;

        public ConvertFromImplementation(ConversionManager manager)
        {
            _manager = manager;
        }

        public T From<T>(TableProfile profile, Dictionary<string, AttributeValue> attrsValues) where T : new()
        {
            T obj = new T();
            var type = typeof(T);
            
            EnsurePrimaryKeyInPlace(profile, attrsValues);
            EnsureIndexesInPlace(profile, attrsValues);

            foreach (var pair in profile.DynamoDbNameToPropName)
            {
                string dynamoDbName = pair.Key;
                string propName = pair.Value;
                
                if (!attrsValues.ContainsKey(dynamoDbName)) continue;
                
                SetValue(type.GetProperty(propName), obj, attrsValues[dynamoDbName]);
            }
            
            // var props = typeof(T).GetProperties();
            // foreach (var prop in props)
            // {
            //     if (prop.GetCustomAttribute<UnmappedAttribute>() is not null) continue;
            //
            //     string propName = GetPropNameAsPerAttributes(prop, out bool byAttribute);
            //
            //     if (!attrsValues.ContainsKey(propName))
            //     {
            //         if (byAttribute) throw new PrimaryKeyInModelNonExistentInDynamoDBException();
            //         continue;
            //     }
            //
            //     SetValue(prop, obj, attrsValues[propName]);
            // }

            return obj;
        }

        private void EnsureIndexesInPlace(TableProfile profile, Dictionary<string, AttributeValue> attrsValues)
        {
            // TODO implement it with LSI
        }

        private void EnsurePrimaryKeyInPlace(TableProfile profile, Dictionary<string, AttributeValue> attrsValues)
        {
            // string dynamoDbPartitionKeyName = profile.PropNameToDynamoDbName[profile.PartitionKeyName];
            
            if (!attrsValues.ContainsKey(profile.PartitionKeyName))
                throw new PrimaryKeyInModelNonExistentInDynamoDBException();
            if (!string.IsNullOrEmpty(profile.SortKeyName) && !attrsValues.
                ContainsKey(profile.SortKeyName))
            {
                throw new PrimaryKeyInModelNonExistentInDynamoDBException();
            }
        }

        private string GetPropNameAsPerAttributes(PropertyInfo prop, out bool primaryKey)
        {
            PartitionKeyAttribute partitionKeyAttribute = null;
            SortKeyAttribute sortKeyAttribute = null;
            AttributeNameAttribute attributeNameAttribute = null;

            foreach (var attribute in prop.GetCustomAttributes())
            {
                if (attribute is PartitionKeyAttribute) partitionKeyAttribute = attribute as PartitionKeyAttribute;
                else if (attribute is SortKeyAttribute) sortKeyAttribute = attribute as SortKeyAttribute;
                else if (attribute is AttributeNameAttribute) attributeNameAttribute = attribute as AttributeNameAttribute;
            }

            primaryKey = partitionKeyAttribute is not null || sortKeyAttribute is not null;

            if (partitionKeyAttribute is not null && !string.IsNullOrEmpty(partitionKeyAttribute.Name)) return partitionKeyAttribute.Name;
            if (sortKeyAttribute is not null && !string.IsNullOrEmpty(sortKeyAttribute.Name)) return sortKeyAttribute.Name;
            if (attributeNameAttribute is not null && !string.IsNullOrEmpty(attributeNameAttribute.Name)) return attributeNameAttribute.Name;
            
            return prop.Name;
        }

        private void SetValue<T>(PropertyInfo prop, T obj, AttributeValue attributeValue)
        {
            if (!_manager.FromAttVal.ContainsKey(prop.PropertyType))
                throw new UnsupportedTypeException(typeof(T));
            
            object propValue = _manager.FromAttVal[prop.PropertyType](attributeValue);
            prop.SetValue(obj, propValue);
        }
    }
}