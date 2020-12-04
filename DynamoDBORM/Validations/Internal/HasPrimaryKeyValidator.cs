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
        }

        private void EnsureSinglePartitionKey(ref Dictionary<Type, AttributeInfo> attributeInfos)
        {
            if (!attributeInfos.ContainsKey(typeof(PartitionKeyAttribute))) throw new NoPartitionKeyException();
            // TODO handle multiples
        }

        private void IfHasSortKeyEnsureASingle(ref Dictionary<Type, AttributeInfo> attributesInfo)
        {
            // TODO handle multiples
        }
    }
}