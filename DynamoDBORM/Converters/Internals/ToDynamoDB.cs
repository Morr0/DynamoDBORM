using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters.Internals
{
    internal static class ToDynamoDB
    {
        internal static Dictionary<Type, Func<object, AttributeValue>> To = new Dictionary<Type, Func<object, AttributeValue>>
        {
            { typeof(string), (obj) => new AttributeValue{ S = obj?.ToString()}},
        };
    }
}