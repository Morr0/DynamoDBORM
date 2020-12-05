using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters.Internals
{
    internal class PrimitivesConverter : BaseConverter
    {
        public override Dictionary<Type, Func<object, AttributeValue>> GetTosMappings => ToDynamoDB.To;
        public override Dictionary<Type, Func<AttributeValue, object>> GetFromsMappings => FromDynamoDB.From;
    }
}