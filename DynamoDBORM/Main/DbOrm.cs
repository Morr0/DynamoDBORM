using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2;
using DynamoDBORM.Attributes;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions;
using DynamoDBORM.Repositories;
using DynamoDBORM.Utilities;
using DynamoDBORM.Validations;
using DynamoDBORM.Validations.Internal;

namespace DynamoDBORM.Main
{
    public class DbOrm
    {
        private readonly IEnumerable<Type> _types;
        private readonly int _typesCount;
        
        private ValidationsPipeline _validationsPipeline;
        
        private ConversionManager _conversionManager;

        private Dictionary<Type, TableProfile> _profiles;
        private IRepository _repository;

        public DbOrm(IEnumerable<Type> types, IEnumerable<BaseValidator> validators, 
            IEnumerable<BaseConverter> converters)
        {
            _types = types;
            _typesCount = (types is List<Type> list) 
                ? list.Count 
                : types.Count();
            
            TakeCareOfConverters(converters);
            TakeCareOfValidations(validators);
            TakeCareOfRepositories();
        }

        private void TakeCareOfRepositories()
        {
            _profiles = new Dictionary<Type, TableProfile>(_typesCount);
            
            foreach (var type in _types)
            {
                _profiles.Add(type, TypeToTableProfile.Get(type));
            }
            _repository = new Repository(_conversionManager, new AmazonDynamoDBClient(), _profiles);
        }
        
        private void TakeCareOfConverters(IEnumerable<BaseConverter> converters)
        {
            _conversionManager = new ConversionManager(converters);
        }

        private void TakeCareOfValidations(IEnumerable<BaseValidator> baseValidators)
        {
            var validators = new List<BaseValidator>
            {
                new HasPrimaryKeyValidator()
            };
            if (baseValidators is not null)
                validators.AddRange(baseValidators);
            _validationsPipeline = new ValidationsPipeline(_conversionManager, validators);

            _validationsPipeline.Validate(_types);
        }
    }
}