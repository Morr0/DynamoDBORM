using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Converters.Internals;

namespace DynamoDBORM.Converters
{
    public sealed class ConversionManager
    {
        private List<BaseConverter> _converters = new List<BaseConverter>
        {
            new PrimitivesConverter()
        };

        private ConvertToImplementation _toImpl;
        private ConvertFromImplementation _fromImpl;

        public ConversionManager(IEnumerable<BaseConverter> converters = null)
        {
            if (converters is not null) _converters.AddRange(converters);
            
            _toImpl = new ConvertToImplementation(_converters);
            _fromImpl = new ConvertFromImplementation(_converters);
        }

        public Dictionary<string, AttributeValue> To<T>(T table)
        {
            return _toImpl.To(table);
        }

        public T From<T>(Dictionary<string, AttributeValue> attrs) where T : new()
        {
            return _fromImpl.From<T>(attrs);
        }
    }
}