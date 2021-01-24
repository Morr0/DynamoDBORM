using System;
using System.Collections.Generic;
using System.Reflection;
using DynamoDBORM.Attributes;

namespace DynamoDBORM.Utilities
{
    // The go to class when requesting any table specific thing
    internal class TableModelUtil
    {
        internal static Dictionary<Type, AttributeInfo> GetAttributesForTypeToTableProfile(ref Type type)
        {
            ISet<Type> atts = new HashSet<Type>
            {
                typeof(PartitionKeyAttribute),
                typeof(SortKeyAttribute),
                typeof(TableAttribute)
            };

            return GetAttributes(ref type, ref atts);
        }

        internal static Dictionary<Type, AttributeInfo> GetAttributes(ref Type type, ref ISet<Type> attributes)
        {
            var dict = new Dictionary<Type, AttributeInfo>();

            foreach (var prop in type.GetProperties())
            {
                foreach (var attribute in prop.GetCustomAttributes())
                {
                    if (attribute is BaseAttribute)
                    {
                        var att = attribute as BaseAttribute;
                        if (attributes.Contains(att.GetType()))
                        {
                            if (!dict.ContainsKey(att.GetType())) dict.Add(att.GetType(), new AttributeInfo(att));
                            else dict[att.GetType()].Count++;

                            if (att is AttributeNameAttribute nameAttribute)
                                nameAttribute.Name = !string.IsNullOrEmpty(nameAttribute.Name)
                                    ? nameAttribute.Name
                                    : prop.Name;
                        }
                    }
                }
            }

            return dict;
        }

        internal static Dictionary<string, string> GetDynamoDbNamesPerPropName(ref Type type)
        {
            var dict = new Dictionary<string, string>();
            foreach (var prop in type.GetProperties())
            {
                foreach (var attribute in prop.GetCustomAttributes())
                {
                    if (attribute is BaseAttribute)
                    {
                        if (attribute is UnmappedAttribute) continue;
                        
                        if (attribute is AttributeNameAttribute ana)
                        {
                            dict.Add(prop.Name, string.IsNullOrEmpty(ana.Name) ? prop.Name : ana.Name);
                        }
                    }
                }
            }

            return dict;
        }
    }

    internal class AttributeInfo
    {
        public AttributeInfo(BaseAttribute attribute)
        {
            Attribute = attribute;
            Count = 1;
        }
        
        public BaseAttribute Attribute { get; }
        
        public int Count { get; set; }
    }
}