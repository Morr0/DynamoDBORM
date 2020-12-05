using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters
{
    public abstract class BaseConverter
    {
        // Template methods
        protected abstract Dictionary<Type, Func<object, AttributeValue>> GetTosMappings();
        protected abstract Dictionary<Type, Func<AttributeValue, object>> GetFromsMappings();

        // Internals
        internal Dictionary<Type, Func<object, AttributeValue>> GetTos() => GetTosMappings();
        internal Dictionary<Type, Func<AttributeValue, object>> GetFroms() => GetFromsMappings();
    }
}