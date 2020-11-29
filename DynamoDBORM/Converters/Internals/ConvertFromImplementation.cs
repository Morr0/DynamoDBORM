using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Attributes;
using DynamoDBORM.Exceptions.Converters;

namespace DynamoDBORM.Converters.Internals
{
    internal class ConvertFromImplementation
    {
        private IEnumerable<BaseConverter> _converters;

        public ConvertFromImplementation(IEnumerable<BaseConverter> converters)
        {
            _converters = converters;
        }

        public T From<T>(Dictionary<string, AttributeValue> attrsValues) where T : new()
        {
            T obj = new T();

            EnsurePrimaryKeyExistsInDynamoDB(obj, attrsValues);
            
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                if (!attrsValues.ContainsKey(prop.Name)) continue;
                if (HasUnmappedAttribute(prop)) continue;

                SetValue(prop, obj, attrsValues[prop.Name]);
            }

            return obj;
        }

        private void SetValue<T>(PropertyInfo prop, T obj, AttributeValue attributeValue) where T : new()
        {
            object propValue = null;
            foreach (var converter in _converters)
            {
                propValue = converter.ConvertFrom(prop, attributeValue);
                if (propValue is null) continue;
                    
                prop.SetValue(obj, propValue);
            }
                
            if (propValue is null) throw new UnsupportedTypeException(typeof(T));
        }

        private bool HasUnmappedAttribute(PropertyInfo prop)
        {
            bool hasUnMapAttribute = false;
            var attributes = prop.GetCustomAttributes();
            foreach (var attribute in attributes)
            {
                if (attribute is UnmappedAttribute)
                {
                    hasUnMapAttribute = true;
                    break;
                }
            }

            return hasUnMapAttribute;
        }

        private void EnsurePrimaryKeyExistsInDynamoDB<T>(T obj, Dictionary<string, AttributeValue> attrsValues) where T : new()
        {
            var tableAttribute = typeof(T).GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;

            if (!attrsValues.ContainsKey(tableAttribute.PartitionKey)) 
                throw new PrimaryKeyInModelNonExistentInDynamoDBException(ConversionExceptionReason.NonExistentPartitionKey);
            else if (!string.IsNullOrEmpty(tableAttribute.SortKey) && !attrsValues.ContainsKey(tableAttribute.SortKey))
                throw new PrimaryKeyInModelNonExistentInDynamoDBException(ConversionExceptionReason.NonExistentSortKey);

        }
    }
}