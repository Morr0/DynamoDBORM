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

            var tableAttribute = table.GetType().GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            string partitionKeyName = tableAttribute.PartitionKey;
            string sortKeyName = tableAttribute.SortKey;
            
            var props = table.GetType().GetProperties();
            foreach (var prop in props)
            {
                var attributes = prop.GetCustomAttributes();
                bool hasUnmap = false;
                bool doNotWriteIfNull = false;
                foreach (var attribute in attributes)
                {
                    if (attribute is UnmappedAttribute)
                    {
                        hasUnmap = true;
                        break;
                    } else if (attribute is DoNotWriteWhenNullAttribute)
                    {
                        doNotWriteIfNull = true;
                        break;
                    }
                }

                if (hasUnmap) continue;

                if (prop.Name == partitionKeyName && prop.GetValue(table) is null)
                    throw new NullPrimaryKeyException(ConversionExceptionReason.NullPartitionKey);
                else if (prop.Name == sortKeyName && prop.GetValue(table) is null)
                    throw new NullPrimaryKeyException(ConversionExceptionReason.NullSortKey);
                else if (doNotWriteIfNull)
                {
                    if (prop.GetValue(table) is null) continue;
                }
                
                AttributeValue attributeValue = null;
                foreach (var converter in _converters)
                {
                    attributeValue = converter.ConvertTo(prop, prop.GetValue(table));
                    if (attributeValue is not null) break;
                }

                if (attributeValue is null) throw new UnsupportedTypeException(prop.PropertyType);
                
                dict.Add(prop.Name, attributeValue);
            }

            return dict;
        }
    }
}