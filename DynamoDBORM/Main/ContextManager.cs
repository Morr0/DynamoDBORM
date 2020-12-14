using System;
using System.Collections.Generic;
using System.Reflection;
using Amazon.DynamoDBv2;
using DynamoDBORM.Converters;
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
                foreach (var prop in context.GetType().GetProperties())
                {
                    if (IsTableObject(prop))
                    {
                        var tableType = prop.PropertyType;
                        var tableModel = tableType.GenericTypeArguments[0];
                        var tableModelObj = Activator.CreateInstance(tableModel);

                        var tableGenerics = tableType.MakeGenericType(tableModel);
                        var table = Activator.CreateInstance(tableGenerics) as Table<object>;
                        table.AddInstance(tableModelObj);
                        table.AddRepository(new Repository(_conversionManager, new AmazonDynamoDBClient()));
                        
                        Console.WriteLine(tableModel.FullName);
                        prop.SetValue(context, table);
                    }
                }
            }
        }
        
        private bool IsTableObject(PropertyInfo prop)
        {
            var generics = prop.PropertyType.GenericTypeArguments;
            return generics.Length == 1;
        }
    }
}