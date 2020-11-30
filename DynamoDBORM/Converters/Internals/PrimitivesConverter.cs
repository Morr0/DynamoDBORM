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

        protected override AttributeValue ConvertTo(PropertyInfo prop, object value)
        {
            return ToDynamoDB.To.ContainsKey(prop.PropertyType) 
                ? ToDynamoDB.To[prop.PropertyType](value) 
                : null;
        }

        protected override object ConvertFrom(PropertyInfo prop, AttributeValue attributeValue)
        {
            return FromDynamoDB.From.ContainsKey(prop.PropertyType)
                ? FromDynamoDB.From[prop.PropertyType](attributeValue)
                : null;
        }
    }
}