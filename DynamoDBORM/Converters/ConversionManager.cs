using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Converters.Internals;

namespace DynamoDBORM.Converters
{
    public sealed class ConversionManager
    {
        private List<BaseConverter> _converters = new()
        {
            new PrimitivesConverter(),
            new GuidConverter()
        };

        private ConvertToImplementation _toImpl;
        private ConvertFromImplementation _fromImpl;

        public ConversionManager(IEnumerable<BaseConverter> converters = null)
        {
            if (converters is not null) _converters.AddRange(converters);
            
            _toImpl = new ConvertToImplementation(this);
            _fromImpl = new ConvertFromImplementation(this);

            PopulateACompleteToFromDicts();
        }

        private void PopulateACompleteToFromDicts()
        {
            foreach (var converter in _converters)
            {
                ToAttVal = ToAttVal.Concat(converter.GetTosMappings).ToDictionary
                    (x => x.Key, x => x.Value);
                FromAttVal = FromAttVal.Concat(converter.GetFromsMappings).ToDictionary
                    (x => x.Key, x => x.Value);
            }
        }

        internal Dictionary<Type, Func<object, AttributeValue>> ToAttVal = new();
        internal Dictionary<Type, Func<AttributeValue, object>> FromAttVal = new();

        public Dictionary<string, AttributeValue> To<T>(T table) => _toImpl.To(table);

        public T From<T>(Dictionary<string, AttributeValue> attrs) where T : new() => _fromImpl.From<T>(attrs);
    }
}