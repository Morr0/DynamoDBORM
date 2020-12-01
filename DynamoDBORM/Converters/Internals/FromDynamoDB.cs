using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Amazon.DynamoDBv2.Model;

[assembly: InternalsVisibleTo("DynamoDBORMTest")]
namespace DynamoDBORM.Converters.Internals
{
    internal static class FromDynamoDB
    {
        internal static Dictionary<Type, Func<AttributeValue, object>> From = new Dictionary<Type, Func<AttributeValue, object>>
        {
            // Strings
            { typeof(string), (val) => val.S},
            { typeof(char), (val) => val.S.ToCharArray()[0]},
            { typeof(char[]), (val) => val.S.ToCharArray()},
            // Byte
            { typeof(sbyte), (val) => sbyte.Parse(val.N)},
            { typeof(byte), (val) => byte.Parse(val.N)},
            // Signed numbers
            { typeof(short), (val) => short.Parse(val.N)},
            { typeof(int), (val) => int.Parse(val.N)},
            { typeof(long), (val) => long.Parse(val.N)},
            { typeof(float), (val) => float.Parse(val.N)},
            { typeof(double), (val) => double.Parse(val.N)},
            { typeof(decimal), (val) => decimal.Parse(val.N)},
            // Unsigned integers
            { typeof(ushort), (val) => ushort.Parse(val.N)},
            { typeof(uint), (val) => uint.Parse(val.N)},
            { typeof(ulong), (val) => ulong.Parse(val.N)},
            // Bool
            { typeof(bool), (val) => val.BOOL}
        };
    }
}