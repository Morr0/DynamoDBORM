using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters
{
    public abstract class BaseConverter
    {
        public virtual AttributeValue ConvertTo(PropertyInfo prop, object value)
        {
            return new AttributeValue();
        }

        internal AttributeValue ProcessTo(PropertyInfo prop, object value)
        {
            return ConvertTo(prop, value);
        }
    }
}