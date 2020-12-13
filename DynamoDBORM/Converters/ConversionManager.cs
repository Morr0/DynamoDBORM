using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Converters.Internals;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Converters
{
    public sealed class ConversionManager
    {
        private List<BaseConverter> _converters = new List<BaseConverter>
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
                ToAttVal.AddOther(converter.GetTosMappings);
                FromAttVal.AddOther(converter.GetFromsMappings);
            }
        }

        internal Dictionary<Type, Func<object, AttributeValue>> ToAttVal { get; } = new Dictionary<Type, Func<object, AttributeValue>>();
        internal Dictionary<Type, Func<AttributeValue, object>> FromAttVal { get; } = new Dictionary<Type, Func<AttributeValue, object>>();

        public Dictionary<string, AttributeValue> To<T>(T table)
        {
            return _toImpl.To(table);
        }

        public T From<T>(Dictionary<string, AttributeValue> attrs) where T : new()
        {
            return _fromImpl.From<T>(attrs);
        }

        internal void ConstructProfiles(ref Dictionary<Type, TableProfile> profiles)
        {
            TableProfiles.Profiles = profiles;
        }
    }
}