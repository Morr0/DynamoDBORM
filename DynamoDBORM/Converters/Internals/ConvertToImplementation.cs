using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Attributes;
using DynamoDBORM.Exceptions.Converters;
using DynamoDBORM.Exceptions.Validations;

namespace DynamoDBORM.Converters.Internals
{
    internal class ConvertToImplementation
    {
        private ConversionManager _manager;

        public ConvertToImplementation(ConversionManager manager)
        {
            _manager = manager;
        }
        
        public Dictionary<string, AttributeValue> To<T>(T table)
        {
            var dict = new Dictionary<string, AttributeValue>();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.GetCustomAttribute<UnmappedAttribute>() is not null) continue;
                
                string propName = prop.Name;
                var partitionKeyAttribute = prop.GetCustomAttribute<PartitionKeyAttribute>();
                var sortKeyAttribute = prop.GetCustomAttribute<SortKeyAttribute>();
                var attributeNameAttribute = prop.GetCustomAttribute<AttributeNameAttribute>();

                if (partitionKeyAttribute is not null)
                {
                    propName = !string.IsNullOrEmpty(partitionKeyAttribute.Name) ? partitionKeyAttribute.Name : propName;
                    if (prop.GetValue(table) is null) throw new NullPartitionKeyException();
                } else if (sortKeyAttribute is not null)
                {
                    propName = !string.IsNullOrEmpty(sortKeyAttribute.Name) ? sortKeyAttribute.Name : propName;
                    if (prop.GetValue(table) is null) throw new NullSortKeyException();
                }
                else if (attributeNameAttribute is not null)
                {
                    propName = !string.IsNullOrEmpty(attributeNameAttribute.Name)
                        ? attributeNameAttribute.Name
                        : propName;
                }

                dict.Add(propName, GetAttribute(prop, table));
            }

            return dict;
        }

        private AttributeValue GetAttribute<T>(PropertyInfo prop, T table)
        {
            AttributeValue attributeValue = _manager.ToAttVal[prop.PropertyType](prop.GetValue(table));
            if (attributeValue == null) throw new UnsupportedTypeException(prop.PropertyType);
            
            return attributeValue;
        }
    }
}