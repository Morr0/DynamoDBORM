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

        public virtual object ConvertFrom(PropertyInfo prop, AttributeValue attributeValue)
        {
            return new object();
        }

        internal object ProcessFrom(PropertyInfo prop, AttributeValue attributeValue)
        {
            return ConvertFrom(prop, attributeValue);
        }
    }
}