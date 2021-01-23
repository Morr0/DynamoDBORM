using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters.Internals
{
    // TODO deal with empty lists
    internal class CollectionsConverter : BaseConverter
    {
        public override Dictionary<Type, Func<object, AttributeValue>> GetTosMappings =>
            new Dictionary<Type, Func<object, AttributeValue>>
            {
                { typeof(List<string>), o => new AttributeValue{ SS = o as List<string>}},
                { typeof(List<int>), o => new AttributeValue{ NS = ToListOfStrings(o as List<int>)}},
                { typeof(List<long>), o => new AttributeValue{ NS = ToListOfStrings(o as List<long>)}},
                { typeof(List<float>), o => new AttributeValue{ NS = ToListOfStrings(o as List<float>)}},
                { typeof(List<double>), o => new AttributeValue{ NS = ToListOfStrings(o as List<double>)}},
                { typeof(List<decimal>), o => new AttributeValue{ NS = ToListOfStrings(o as List<decimal>)}},
            };

        private List<string> ToListOfStrings(object obj)
        {
            if (obj is List<int> ints)
            {
                return ints.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList();
            }
            if (obj is List<long> longs)
            {
                return longs.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList();
            }
            if (obj is List<float> floats)
            {
                return floats.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList();
            }
            if (obj is List<double> doubles)
            {
                return doubles.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList();
            }
            if (obj is List<decimal> decimals)
            {
                return decimals.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList();
            }

            return new List<string>();
        }

        public override Dictionary<Type, Func<AttributeValue, object>> GetFromsMappings =>
            new Dictionary<Type, Func<AttributeValue, object>>
            {
                { typeof(List<string>), val => val.SS},
                { typeof(List<int>), val => val.NS}
            };

        private object FromStringsToTheirRespectiveTypeList<T>(List<string> numbers)
        {
            if (typeof(T) == typeof(int))
            {
                return numbers.Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToList();
            }
            if (typeof(T) == typeof(long))
            {
                return numbers.Select(x => long.Parse(x, CultureInfo.InvariantCulture)).ToList();
            }
            if (typeof(T) == typeof(float))
            {
                return numbers.Select(x => float.Parse(x, CultureInfo.InvariantCulture)).ToList();
            }
            if (typeof(T) == typeof(double))
            {
                return numbers.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            }
            if (typeof(T) == typeof(decimal))
            {
                return numbers.Select(x => decimal.Parse(x, CultureInfo.InvariantCulture)).ToList();
            }

            return null;
        }
    }
}