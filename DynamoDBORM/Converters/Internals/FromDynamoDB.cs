using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters.Internals
{
    internal static class FromDynamoDB
    {
        internal static Dictionary<Type, Func<AttributeValue, object>> To = new Dictionary<Type, Func<AttributeValue, object>>
        {
            { typeof(string), (val) => val.S},
        };
    }
}