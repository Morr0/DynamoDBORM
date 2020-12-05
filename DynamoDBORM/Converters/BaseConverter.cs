using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters
{
    public abstract class BaseConverter
    {
        // Template methods
        public abstract Dictionary<Type, Func<object, AttributeValue>> GetTosMappings
        {
            get;
        }

        public abstract Dictionary<Type, Func<AttributeValue, object>> GetFromsMappings
        {
            get;
        }
    }
}