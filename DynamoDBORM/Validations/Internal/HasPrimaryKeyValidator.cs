using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using DynamoDBORM.Attributes;
using DynamoDBORM.Exceptions;
using DynamoDBORM.Exceptions.Validations;
using DynamoDBORM.Utilities;

[assembly: InternalsVisibleTo("DynamoDBORMTest")]
namespace DynamoDBORM.Validations.Internal
{
    internal class HasPrimaryKeyValidator : BaseValidator
    {
        protected override void Validate(ref object model, ref ISet<Type> attributes)
        {
            var type = model.GetType();
            var attributesInfo = TableModelUtil.GetAttributes(ref type, ref attributes);
            EnsureSinglePartitionKey(ref attributesInfo);
            IfHasSortKeyEnsureASingle(ref attributesInfo);
            EnsurePrimaryKeysDontHaveAttributeNameAttribute(ref attributesInfo);
        }

        private void EnsurePrimaryKeysDontHaveAttributeNameAttribute(ref Dictionary<Type, AttributeInfo> dict)
        {
            if ((dict.ContainsKey(typeof(PartitionKeyAttribute)) || dict.ContainsKey(typeof(SortKeyAttribute)))
                || dict.ContainsKey(typeof(AttributeNameAttribute)))
                throw new PrimaryKeyShouldBeWithoutAttributeNamedAttributeException();
        }

        private void EnsureSinglePartitionKey(ref Dictionary<Type, AttributeInfo> attributeInfos)
        {
            if (!attributeInfos.ContainsKey(typeof(PartitionKeyAttribute))) throw new NoPartitionKeyException();

            var info = attributeInfos[typeof(PartitionKeyAttribute)];
            if (info.Count > 1) throw new MultiplePartitionKeysException();
        }

        private void IfHasSortKeyEnsureASingle(ref Dictionary<Type, AttributeInfo> attributesInfo)
        {
            if (attributesInfo.ContainsKey(typeof(SortKeyAttribute)))
            {
                var info = attributesInfo[typeof(SortKeyAttribute)];
                if (info.Count > 1) throw new MultipleSortKeysException();
            }
        }
    }
}