using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using DynamoDBORM.Converters;
using DynamoDBORM.Repositories;
using DynamoDBORM.Utilities;
using DynamoDBORM.Validations;
using DynamoDBORM.Validations.Internal;

namespace DynamoDBORM.Main
{
    public class DbOrm
    {
        private readonly DataContext[] _contexts;
        private readonly IEnumerable<Type> _types;
        
        private ValidationsPipeline _validationsPipeline;
        
        private ConversionManager _conversionManager;

        private Dictionary<Type, TableConfiguration> _tableConfigurations;
        private Dictionary<Type, IRepository> _repositories;
        private Dictionary<Type, TableProfile> _profiles;

        private DataContext[] _dataContexts;

        public DbOrm(DataContext[] contexts, IEnumerable<BaseValidator> validators, IEnumerable<BaseConverter> converters)
        {
            _contexts = contexts;
            SetValidators(validators);
            SetConverters(converters);
            
            
            foreach (var context in contexts)
            {
                TakeCareOfDataContext(context);
            }
        }

        private void TakeCareOfDataContext(DataContext dataContext)
        {
            var contextProps = dataContext.GetType().GetProperties();
            _tableConfigurations = new Dictionary<Type, TableConfiguration>(contextProps.Length);
            var types = new List<Type>(contextProps.Length);

            foreach (var prop in contextProps)
            {
                if (prop.PropertyType == typeof(Table<object, TableConfiguration>))
                {
                    var genericTypeArguments = prop.GetType().GenericTypeArguments;
                    // The indexes are according to the Table class
                    var type = genericTypeArguments[0];
                    var configPerTypeTable = genericTypeArguments[1];
                    
                    types.Add(type);
                    _tableConfigurations.Add(type, CreateConfig(configPerTypeTable));
                }
            }
            
            EnsureTypesAreValid();
            TakeCareOfRepositories(ref types);
        }

        private TableConfiguration CreateConfig(Type configPerTypeTable)
        {
            return Activator.CreateInstance(configPerTypeTable) as TableConfiguration;
        }

        private void EnsureTypesAreValid()
        {
            _validationsPipeline.Validate(_types);
        }

        private void TakeCareOfRepositories(ref List<Type> types)
        {
            _repositories = new Dictionary<Type, IRepository>(types.Count);
            _profiles = new Dictionary<Type, TableProfile>(types.Count);
            
            foreach (var type in types)
            {
                _repositories.Add(type, GetRepository(type));
                _profiles.Add(type, TypeToTableProfile.Get(type));
            }
            _conversionManager.ConstructProfiles(ref _profiles);
        }

        private IRepository GetRepository(Type type)
        {
            return new Repository(_conversionManager, new AmazonDynamoDBClient());
        }

        private void SetConverters(IEnumerable<BaseConverter> converters)
        {
            _conversionManager = new ConversionManager(converters);
        }

        private void SetValidators(IEnumerable<BaseValidator> baseValidators)
        {
            var validators = new List<BaseValidator>
            {
                new HasPrimaryKeyValidator()
            };
            if (baseValidators is not null)
                validators.AddRange(baseValidators);
            _validationsPipeline = new ValidationsPipeline(_conversionManager, validators);
        }
    }
}