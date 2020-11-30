using System;
using System.Reflection;
using DynamoDBORM.Attributes;
using DynamoDBORM.Repositories;

namespace DynamoDBORM.Utilities
{
    internal static class TypeToTableProfile
    {
        public static TableProfile Get(Type type)
        {
            // TODO clean this mess
            TableAttribute tableAttribute = null;
            var attrs = type.GetCustomAttributes();
            foreach (var attr in attrs)
            {
                if (attr is TableAttribute)
                {
                    tableAttribute = attr as TableAttribute;
                    break;
                }
            }

            string partitionKeyDbName = tableAttribute.PartitionKey;
            bool hasSortKey = !string.IsNullOrEmpty(tableAttribute.SortKey);
            string sortKeyDbName = tableAttribute.SortKey;

            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name == partitionKeyDbName)
                {
                    var propAttrs = prop.GetCustomAttributes();
                    foreach (var propAttr in propAttrs)
                    {
                        if (propAttr is AttributeNameAttribute)
                        {
                            string name = (propAttr as AttributeNameAttribute).Name;
                            if (!string.IsNullOrEmpty(name)) partitionKeyDbName = name;
                        }
                    }
                } else if (hasSortKey && prop.Name == sortKeyDbName)
                {
                    var propAttrs = prop.GetCustomAttributes();
                    foreach (var propAttr in propAttrs)
                    {
                        if (propAttr is AttributeNameAttribute)
                        {
                            string name = (propAttr as AttributeNameAttribute).Name;
                            if (!string.IsNullOrEmpty(name)) sortKeyDbName = name;
                        }
                    }
                }
            }

            string tableName = !string.IsNullOrEmpty(tableAttribute?.Name) 
                ? tableAttribute.Name 
                : type.Name;
            
            return new TableProfile(tableName, partitionKeyDbName, sortKeyDbName);
        }
    }
}