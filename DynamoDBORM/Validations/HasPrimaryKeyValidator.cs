using System;
using System.Reflection;
using DynamoDBORM.Attributes;
using DynamoDBORM.Exceptions;

namespace DynamoDBORM.Validations
{
    public class HasPrimaryKeyValidator : BaseValidator
    {
        protected override void Validate<T>(T table)
        {
            var type = table.GetType();
            var props = type.GetProperties();
            
            TableAttribute attribute = GetTableAttribute(type);
            HasExistingPartitionKey(attribute, props);
            HasDefinedSortKeyAndIsCorrect(attribute, props);
        }

        private void HasDefinedSortKeyAndIsCorrect(TableAttribute attribute, PropertyInfo[] propertyInfos)
        {
            if (string.IsNullOrEmpty(attribute.SortKey)) return;
            
            bool hasFoundPropNameMatchingSortKeyNameSpecified = false;
            foreach (var prop in propertyInfos)
            {
                if (prop.Name == attribute.SortKey)
                {
                    hasFoundPropNameMatchingSortKeyNameSpecified = true;
                    break;
                }
            }
            
            if (!hasFoundPropNameMatchingSortKeyNameSpecified)
                throw new TableHasNotSpecifiedPartitionKey(Reason.ReferencedNonExistentSortKey);
        }

        private void HasExistingPartitionKey(TableAttribute attribute, PropertyInfo[] propertyInfos)
        {
            if (string.IsNullOrEmpty(attribute.PartitionKey)) throw new TableHasNotSpecifiedPartitionKey();

            bool hasFoundPropNameMatchingPartitionKeyNameSpecified = false;
            foreach (var prop in propertyInfos)
            {
                if (prop.Name == attribute.PartitionKey)
                {
                    hasFoundPropNameMatchingPartitionKeyNameSpecified = true;
                    break;
                }
            }
            
            if (!hasFoundPropNameMatchingPartitionKeyNameSpecified)
                throw new TableHasNotSpecifiedPartitionKey(Reason.ReferencedNonExistentPartitionKey);

        }

        private TableAttribute GetTableAttribute(Type type)
        {
            var classAttrs = type.GetCustomAttributes();
            TableAttribute attribute = null;
            
            foreach (var attr in classAttrs)
            {
                if (attr is TableAttribute)
                {
                    attribute = attr as TableAttribute;
                    break;
                }
            }

            return attribute ?? throw new NoTableAttributeException();
        }
    }
}