﻿using System;
using System.Collections.Generic;
using System.Reflection;
using DynamoDBORM.Attributes;
using DynamoDBORM.Exceptions.Validations;

namespace DynamoDBORM.Validations
{
    public class ValidationsPipeline
    {
        private ISet<Type> _attributes;
        private readonly IEnumerable<BaseValidator> _validators;

        public ValidationsPipeline(IEnumerable<BaseValidator> validators)
        {
            PopulateBaseAttributes();
            _validators = validators;
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