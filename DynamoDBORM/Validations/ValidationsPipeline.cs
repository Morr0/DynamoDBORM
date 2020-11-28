using System;
using System.Collections.Generic;
using DynamoDBORM.Exceptions;

namespace DynamoDBORM.Validations
{
    public class ValidationsPipeline
    {
        private IEnumerable<BaseValidator> _validators;

        public ValidationsPipeline(IEnumerable<BaseValidator> validators)
        {
            _validators = validators;
        }

        public void Validate(IEnumerable<Type> tablesTypes)
        {
            foreach (var type in tablesTypes)
            {
                if (HasNotParameterlessConstructor(type)) throw new NoPublicParameterlessConstructorException();

                var obj = Activator.CreateInstance(type);
                
                foreach (var validator in _validators)
                {
                    validator.ProcessValidation(obj);
                }
            }
        }

        private bool HasNotParameterlessConstructor(Type type)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);
            return constructor is not null;
        }
    }
}