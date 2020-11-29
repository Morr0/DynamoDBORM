using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Attributes;

namespace DynamoDBORM.Converters.Internals
{
    internal class PrimitivesConverter : BaseConverter
    {
        public override AttributeValue ConvertTo(PropertyInfo prop, object value)
        {
            return ToDynamoDB.To.ContainsKey(prop.PropertyType) ? ToDynamoDB.To[prop.PropertyType](value) : null;
        }
    }
}