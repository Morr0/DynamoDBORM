using System;
using System.Collections.Generic;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions.Converters;
using DynamoDBORM.Exceptions.Validations;

namespace DynamoDBORM.Validations.Internal
{
    internal class HasOnlySupportedTypesValidator
    {
        public static void Ensure(ref ConversionManager conversionManager, ref object model)
        {
            foreach (var prop in model.GetType().GetProperties())
            {
                if (!conversionManager.FromAttVal.ContainsKey(prop.PropertyType) && !conversionManager.ToAttVal.ContainsKey(prop.PropertyType))
                    throw new UnsupportedTypeException(prop.PropertyType);
            }
        }
    }
}