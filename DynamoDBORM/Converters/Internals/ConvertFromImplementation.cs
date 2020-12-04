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
            foreach (var prop in props)
            {
                if (prop.GetCustomAttribute<UnmappedAttribute>() is not null) continue;
                
                string propName = prop.Name;
                
                var unmappedAttribute = prop.GetCustomAttribute<UnmappedAttribute>();
                if (unmappedAttribute is not null) continue;
                if (!attrsValues.ContainsKey(propName)) continue;
                
                var partitionKeyAttribute = prop.GetCustomAttribute<PartitionKeyAttribute>();
                var sortKeyAttribute = prop.GetCustomAttribute<SortKeyAttribute>();
                var attributeNameAttribute = prop.GetCustomAttribute<AttributeNameAttribute>();
                
                if (attributeNameAttribute is not null && !string.IsNullOrEmpty(attributeNameAttribute.Name))
                {
                    propName = attributeNameAttribute.Name;
                }
                else if (partitionKeyAttribute is not null && !string.IsNullOrEmpty(partitionKeyAttribute.Name))
                {
                    propName = partitionKeyAttribute.Name;
                    if (prop.Name == partitionKeyAttribute.Name && !attrsValues.ContainsKey(propName))
                        throw new NullPartitionKeyException();
                } else if (sortKeyAttribute is not null && !string.IsNullOrEmpty(sortKeyAttribute.Name))
                {
                    propName = sortKeyAttribute.Name;
                    if (prop.Name == sortKeyAttribute.Name && !string.IsNullOrEmpty(sortKeyAttribute.Name) &&
                        !attrsValues.ContainsKey(propName))
                        throw new NullSortKeyException();
                }

                SetValue(prop, obj, attrsValues[prop.Name]);
            }

            return obj;
        }

        private void SetValue<T>(PropertyInfo prop, T obj, AttributeValue attributeValue)
        {
            object propValue = null;
            foreach (var converter in _converters)
            {
                propValue = converter.ProcessFrom(prop, attributeValue);
                if (propValue is null) continue;
                    
                prop.SetValue(obj, propValue);
            }
                
            if (propValue is null) throw new UnsupportedTypeException(typeof(T));
        }
    }
}