using System;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions.Validations;

namespace DynamoDBORM.Validations.Internal
{
    internal static class HasOnlySupportedTypesValidator
    {
        public static void Ensure(ref ConversionManager conversionManager, ref object model)
        {
            foreach (var prop in model.GetType().GetProperties())
            {
                // TODO fix this bug here
                // if (!conversionManager.FromAttVal.ContainsKey(prop.PropertyType) || !conversionManager.ToAttVal.ContainsKey(prop.PropertyType))
                //     throw new UnsupportedTypeException(prop.PropertyType);
            }
        }
    }
}