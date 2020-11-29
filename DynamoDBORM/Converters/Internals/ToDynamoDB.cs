using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBORM.Converters.Internals
{
    internal static class ToDynamoDB
    {
        internal static Dictionary<Type, Func<object, AttributeValue>> To = new Dictionary<Type, Func<object, AttributeValue>>
        {
            // Strings
            { typeof(char), (obj) => new AttributeValue{ S = obj?.ToString()}},
            { typeof(string), (obj) => new AttributeValue{ S = obj?.ToString()}},
            { typeof(char[]), (obj) => new AttributeValue{ S = new string((char[])obj)}},
            // Byte
            { typeof(byte), (obj) => new AttributeValue{ N = obj?.ToString()}},
            { typeof(sbyte), (obj) => new AttributeValue{ N = obj?.ToString()}},
            // Signed numbers
            { typeof(short), (obj) => new AttributeValue{ N = obj?.ToString()}},
            { typeof(int), (obj) => new AttributeValue{ N = obj?.ToString()}},
            { typeof(long), (obj) => new AttributeValue{ N = obj?.ToString()}},
            { typeof(float), (obj) => new AttributeValue{ N = obj?.ToString()}},
            { typeof(double), (obj) => new AttributeValue{ N = obj?.ToString()}},
            { typeof(decimal), (obj) => new AttributeValue{ N = obj?.ToString()}},
            // Unsigned integers
            { typeof(ushort), (obj) => new AttributeValue{ N = obj?.ToString()}},
            { typeof(uint), (obj) => new AttributeValue{ N = obj?.ToString()}},
            { typeof(ulong), (obj) => new AttributeValue{ N = obj?.ToString()}},
            // Bool
            { typeof(bool), (obj) => new AttributeValue{ BOOL = (bool)obj}}
        };
    }
}