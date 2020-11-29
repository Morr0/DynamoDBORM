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
            
            var tableAttribute = typeof(T).GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;

            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                string propName = prop.Name;
                
                bool hasUnMapAttribute = false;
                var attributes = prop.GetCustomAttributes();
                foreach (var attribute in attributes)
                {
                    if (attribute is UnmappedAttribute)
                    {
                        hasUnMapAttribute = true;
                        break;
                    }
                    
                    if (attribute is AttributeNameAttribute nameAttribute)
                    {
                        propName = nameAttribute?.Name;
                    }
                }
                
                EnsureIfAnyOfPrimaryKeysDoExist(propName, prop, tableAttribute, attrsValues);
                
                if (hasUnMapAttribute) continue;
                
                if (!attrsValues.ContainsKey(propName)) continue;

                SetValue(prop, obj, attrsValues[propName]);
            }

            return obj;
        }

        private void EnsureIfAnyOfPrimaryKeysDoExist(string propName, PropertyInfo prop, 
            TableAttribute tableAttribute, Dictionary<string, AttributeValue> attrsValues)
        {
            if (prop.Name == tableAttribute.PartitionKey && !attrsValues.ContainsKey(propName))
                throw new PrimaryKeyInModelNonExistentInDynamoDBException(ConversionExceptionReason.NonExistentPartitionKey);

            else if (prop.Name == tableAttribute.SortKey && !string.IsNullOrEmpty(tableAttribute.SortKey) &&
                     !attrsValues.ContainsKey(propName))
                throw new PrimaryKeyInModelNonExistentInDynamoDBException(ConversionExceptionReason.NonExistentSortKey);
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
    }
}