using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters.Internals
{
    public class ConvertFromImplementation
    {
        private IEnumerable<BaseConverter> _converters;

        public ConvertFromImplementation(IEnumerable<BaseConverter> converters)
        {
            _converters = converters;
        }

        public T From<T>(Dictionary<string, AttributeValue> attrs) where T : new()
        {
            return new T();
        }
    }
}