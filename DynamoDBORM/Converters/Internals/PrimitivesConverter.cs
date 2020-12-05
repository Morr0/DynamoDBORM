using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters.Internals
{
    internal class PrimitivesConverter : BaseConverter
    {
        protected override Dictionary<Type, Func<object, AttributeValue>> GetTosMappings()
        {
            return ToDynamoDB.To;
        }

        protected override Dictionary<Type, Func<AttributeValue, object>> GetFromsMappings()
        {
            return FromDynamoDB.From;
        }
    }
}