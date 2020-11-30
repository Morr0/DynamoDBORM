using System.Reflection;
using System.Runtime.CompilerServices;
using DynamoDBORM.Attributes;
using DynamoDBORM.Exceptions;
using DynamoDBORM.Exceptions.Validations;

[assembly: InternalsVisibleTo("DynamoDBORMTest")]
namespace DynamoDBORM.Validations.Internal
{
    internal class HasPrimaryKeyValidator : BaseValidator
    {
        protected override void Validate<T>(T table)
        {
            TableAttribute attribute = GetTableAttribute(table);
            HasExistingPartitionKey(attribute, table);
            HasDefinedSortKeyAndIsCorrect(attribute, table);
        }

        private void HasDefinedSortKeyAndIsCorrect<T>(TableAttribute attribute, T table) where T : new()
        {
            if (string.IsNullOrEmpty(attribute.SortKey)) return;

            if (!HasPropertyNamed(table, attribute.SortKey))
                throw new TableHasNotSpecifiedPartitionKey(Reason.ReferencedNonExistentSortKey);
        }

        private void HasExistingPartitionKey<T>(TableAttribute attribute, T table) where T : new()
        {
            if (string.IsNullOrEmpty(attribute.PartitionKey)) throw new TableHasNotSpecifiedPartitionKey();

            if (!HasPropertyNamed(table, attribute.PartitionKey))
                throw new TableHasNotSpecifiedPartitionKey(Reason.ReferencedNonExistentPartitionKey);
        }

        private TableAttribute GetTableAttribute<T>(T table) where T : new()
        {
            var type = table.GetType();
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