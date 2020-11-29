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
            var props = typeof(T).GetProperties();
            
            var tableAttribute = typeof(T).GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;

            if (!attrsValues.ContainsKey(tableAttribute.PartitionKey)) 
                throw new PrimaryKeyInModelNonExistentInDynamoDBException(ConversionExceptionReason.NonExistentPartitionKey);
            else if (!string.IsNullOrEmpty(tableAttribute.SortKey) && !attrsValues.ContainsKey(tableAttribute.SortKey))
                throw new PrimaryKeyInModelNonExistentInDynamoDBException(ConversionExceptionReason.NonExistentSortKey);

            foreach (var prop in props)
            {
                if (!attrsValues.ContainsKey(prop.Name)) continue;

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

                if (hasUnMapAttribute) continue;

                object propValue = null;
                foreach (var converter in _converters)
                {
                    propValue = converter.ConvertFrom(prop, attrsValues[prop.Name]);
                    if (propValue is null) continue;
                    
                    prop.SetValue(obj, propValue);
                }
                
                if (propValue is null) throw new UnsupportedTypeException(typeof(T));
            }

            return obj;
        }
    }
}