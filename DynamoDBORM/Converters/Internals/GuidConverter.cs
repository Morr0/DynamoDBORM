using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters.Internals
{
    public class GuidConverter : BaseConverter
    {
        public override Dictionary<Type, Func<object, AttributeValue>> GetTosMappings => 
            new Dictionary<Type, Func<object, AttributeValue>>
        {
            { typeof(Guid), o => new AttributeValue{ S = o.ToString()}}
        };
        public override Dictionary<Type, Func<AttributeValue, object>> GetFromsMappings => 
            new Dictionary<Type, Func<AttributeValue, object>>
        {
            { typeof(Guid), value => Guid.Parse(value.S)}
        };
    }
}