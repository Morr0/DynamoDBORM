using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions.Validations;
using DynamoDBORM.Repositories;
using DynamoDBORM.Utilities;
using DynamoDBORM.Validations;
using DynamoDBORM.Validations.Internal;

namespace DynamoDBORM.Main
{
    public class DbOrm
    {
        private readonly DataContext[] _contexts;
        private List<Type> _types;
        
        private ValidationsPipeline _validationsPipeline;
        
        private ConversionManager _conversionManager;

        private Dictionary<Type, string> _typeToContextNames;
        private Dictionary<Type, TableConfiguration> _tableConfigurations;
        private Dictionary<Type, IRepository> _repositories;
        private Dictionary<Type, TableProfile> _profiles;
        
        private Dictionary<string, DataContext> _dataContexts;

        public DbOrm(DataContext[] contexts, IEnumerable<BaseValidator> validators, IEnumerable<BaseConverter> converters)
        {
            _contexts = contexts;
            SetValidators(validators);
            SetConverters(converters);
            _typeToContextNames = new Dictionary<Type, string>();
            _tableConfigurations = new Dictionary<Type, TableConfiguration>();
            _types = new List<Type>();
            _dataContexts = new Dictionary<string, DataContext>(contexts.Length);

            foreach (var context in contexts)
            {
                _dataContexts.Add(context.GetType().Name, context);
                TakeCareOfDataContext(context);
            }
        }

        private void TakeCareOfDataContext(DataContext dataContext)
        {
            var contextProps = dataContext.GetType().GetProperties();
            foreach (var prop in contextProps)
            {
                if (IsTableObject(prop))
                {
                    Console.WriteLine("True");
                    var genericTypeArguments = prop.PropertyType.GenericTypeArguments;
                    // The indexes are according to the Table class
                    var type = genericTypeArguments[0];
                    var configPerTypeTable = genericTypeArguments[1];
                    
                    _types.Add(type);
                    _tableConfigurations.Add(type, CreateConfig(configPerTypeTable));
                }
            }
            
            EnsureTypesAreValid();
            TakeCareOfRepositories();
            PopulateTablesInContexts();
        }

        private bool IsTableObject(PropertyInfo prop)
        {
            var generics = prop.PropertyType.GenericTypeArguments;
            return generics.Length == 2 && generics[1] == typeof(TableConfiguration);
        }

        private void PopulateTablesInContexts()
        {
            foreach (var pair in _typeToContextNames)
            {
                var type = pair.Key;
                string contextName = pair.Value;

                var context = _dataContexts[contextName];
                var table = Activator.CreateInstance<Table<object, TableConfiguration>>();
                if (table is null) throw new NoPublicParameterlessConstructorException();
                table.AddRepository(_repositories[type]);
                
                Console.WriteLine("Here");
                // context.GetType().GetProperty(type.Name).SetValue(context, table);
                foreach (var prop in context.GetType().GetProperties())
                {
                    if (prop.PropertyType.Name == type.Name)
                    {
                        prop.SetValue(context, table);
                    }
                }
            }
        }

        private TableConfiguration CreateConfig(Type configPerTypeTable)
        {
            return Activator.CreateInstance(configPerTypeTable) as TableConfiguration;
        }

        private void EnsureTypesAreValid()
        {
            _validationsPipeline.Validate(_types);
        }

        private void TakeCareOfRepositories()
        {
            _repositories = new Dictionary<Type, IRepository>(_types.Count);
            _profiles = new Dictionary<Type, TableProfile>(_types.Count);
            
            foreach (var type in _types)
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