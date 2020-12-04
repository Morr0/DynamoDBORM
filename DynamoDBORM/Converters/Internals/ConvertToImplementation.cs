using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Attributes;
using DynamoDBORM.Exceptions.Converters;

namespace DynamoDBORM.Converters.Internals
{
    internal class ConvertToImplementation
    {
        private IEnumerable<BaseConverter> _converters;

        public ConvertToImplementation(IEnumerable<BaseConverter> converters)
        {
            _converters = converters;
        }
        
        public Dictionary<string, AttributeValue> To<T>(T table)
        {
            var dict = new Dictionary<string, AttributeValue>();

            var props = table.GetType().GetProperties();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.GetCustomAttribute<UnmappedAttribute>() is not null) continue;
                
                string propName = prop.Name;
                var partitionKeyAttribute = prop.GetCustomAttribute<PartitionKeyAttribute>();
                var sortKeyAttribute = prop.GetCustomAttribute<SortKeyAttribute>();

                if (partitionKeyAttribute is not null)
                {
                    propName = partitionKeyAttribute.Name;
                    if (prop.GetValue(table) is null) throw new NullPrimaryKeyException(ConversionExceptionReason.NullPartitionKey);
                } else if (sortKeyAttribute is not null)
                {
                    propName = sortKeyAttribute.Name;
                    if (prop.GetValue(table) is null) throw new NullPrimaryKeyException(ConversionExceptionReason.NullSortKey);
                }

                dict.Add(propName, GetAttribute(prop, table));
            }

            return dict;
        }

        private AttributeValue GetAttribute<T>(PropertyInfo prop, T table)
        {
            AttributeValue attributeValue = null;
            foreach (var converter in _converters)
            {
                attributeValue = converter.ProcessTo(prop, prop.GetValue(table));
                if (attributeValue is not null) break;
            }
            
            return attributeValue;
        }
    }
}