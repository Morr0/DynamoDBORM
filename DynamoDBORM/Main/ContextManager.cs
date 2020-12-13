using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions.Validations;
using DynamoDBORM.Repositories;

namespace DynamoDBORM.Main
{
    public sealed class ContextManager
    {
        private readonly List<DataContext> _contexts = new List<DataContext>();
        private readonly ConversionManager _conversionManager = new ConversionManager();

        public ContextManager(DataContext context)
        {
            _contexts.Add(context);
            
            PopulateContexts();
        }

        private void PopulateContexts()
        {
            foreach (var context in _contexts)
            {
                foreach (var property in context.GetType().GetProperties())
                {
                    if (IsTableObject(property))
                    {
                        var tableModel = property.PropertyType.GenericTypeArguments[0];
                        var tableModelObj = Activator.CreateInstance(tableModel);
                        var table = Activator.CreateInstance<Table<object>>();
                        table.AddInstance(tableModelObj);
                        table.AddRepository(new Repository(_conversionManager, new AmazonDynamoDBClient()));

                        var prop = table.GetType().GetProperty(property.PropertyType.Name);
                        prop.SetValue(context, table);
                    }
                }
            }
        }
        
        private bool IsTableObject(PropertyInfo prop)
        {
            var generics = prop.PropertyType.GenericTypeArguments;
            return generics.Length == 1 && generics[0] == typeof(TableConfiguration);
        }
    }
}