﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters.Internals
{
    public class GuidConverter : BaseConverter
    {
        protected override Dictionary<Type, Func<object, AttributeValue>> GetTosMappings()
        {
            return new Dictionary<Type, Func<object, AttributeValue>>
            {
                { typeof(Guid), o => new AttributeValue{ S = o.ToString()}}
            };
        }

        protected override Dictionary<Type, Func<AttributeValue, object>> GetFromsMappings()
        {
            return new Dictionary<Type, Func<AttributeValue, object>>
            {
                { typeof(Guid), value => Guid.Parse(value.S)}
            };
        }

        protected override AttributeValue ConvertTo(PropertyInfo prop, object value)
        {
            var map = GetTosMappings();
            return map.ContainsKey(prop.PropertyType) ? map[prop.PropertyType](value) : null;
        }

        protected override object ConvertFrom(PropertyInfo prop, AttributeValue attributeValue)
        {
            var map = GetFromsMappings();
            return map.ContainsKey(prop.PropertyType) ? map[prop.PropertyType](attributeValue) : null;
        }
    }
}