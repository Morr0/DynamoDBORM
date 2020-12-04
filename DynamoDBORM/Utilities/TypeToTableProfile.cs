using System;
using DynamoDBORM.Attributes;
using DynamoDBORM.Repositories;

namespace DynamoDBORM.Utilities
{
    internal static class TypeToTableProfile
    {
        public static TableProfile Get(Type type)
        {
            var dict = TableModelUtil.GetAttributesForTypeToTableProfile(ref type);

            string tableName = type.Name;
            if (dict.ContainsKey(typeof(TableAttribute)))
            {
                var tableAttribute = dict[typeof(TableAttribute)].Attribute as TableAttribute;
                if (!string.IsNullOrEmpty(tableAttribute?.Name)) tableName = tableAttribute.Name;
            }
            
            string partitionKeyName = (dict[typeof(PartitionKeyAttribute)].Attribute as PartitionKeyAttribute)?.Name;
            
            string sortKeyName = null;
            if (dict.ContainsKey(typeof(SortKeyAttribute)))
            {
                var sortKeyAttribute = dict[typeof(SortKeyAttribute)].Attribute as SortKeyAttribute;
                if (!string.IsNullOrEmpty(sortKeyAttribute.Name)) sortKeyName = sortKeyAttribute.Name;
            }
            
            return new TableProfile(tableName, partitionKeyName, sortKeyName);
        }
    }
}