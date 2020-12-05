using System;
using System.Collections.Generic;
using DynamoDBORM.Attributes;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions.Validations;
using DynamoDBORM.Validations.Internal;

namespace DynamoDBORM.Validations
{
    public class ValidationsPipeline
    {
        private ISet<Type> _attributes;
        private ConversionManager _conversionManager;
        private readonly IEnumerable<BaseValidator> _validators;

        public ValidationsPipeline(ConversionManager conversionManager, IEnumerable<BaseValidator> CustomValidators)
        {
            PopulateBaseAttributes();
            _conversionManager = conversionManager;
            _validators = CustomValidators;
        }

        private void PopulateBaseAttributes()
        {
            _attributes = new HashSet<Type>
            {
                typeof(AttributeNameAttribute),
                typeof(PartitionKeyAttribute),
                typeof(SortKeyAttribute),
                typeof(TableAttribute),
                typeof(UnmappedAttribute)
            };
        }

        public void Validate(IEnumerable<Type> tablesTypes)
        {
            foreach (var type in tablesTypes)
            {
                if (HasNotParameterlessConstructor(type)) throw new NoPublicParameterlessConstructorException();
                var obj = Activator.CreateInstance(type);

                HasOnlySupportedTypesValidator.Ensure(ref _conversionManager, ref obj);
                
                foreach (var validator in _validators)
                {
                    validator.ProcessValidation(ref obj, ref _attributes);
                }
            }
        }

        private bool HasNotParameterlessConstructor(Type type)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);
            return constructor is null;
        }
    }
}