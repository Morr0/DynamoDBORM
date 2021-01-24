using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions.Repositories;
using DynamoDBORM.Repositories.Implementation;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Repositories
{
    // TODOS
    // TODO Whether or not to update unmapped values
    // TODO use preferred name when writing to DynamoDB
    
    public class Repository : IRepository
    {
        private readonly ConversionManager _conversionManager;
        private readonly AmazonDynamoDBClient _client;
        private readonly Dictionary<Type, TableProfile> _profiles;
        private readonly RepositoryImpl _impl;

        internal Repository(ConversionManager conversionManager, AmazonDynamoDBClient client)
        {
            _conversionManager = conversionManager;
            _client = client;
            _profiles = new Dictionary<Type, TableProfile>();

            _impl = new RepositoryImpl(_conversionManager);
        }
        
        public async Task<T> Get<T>(object partitionKey, object sortKey = null) where T : new()
        {
            var profile = EnsureProfile<T>();
            var dict = await _impl.Get<T>(_client, profile, partitionKey, sortKey).ConfigureAwait(false);
            return _conversionManager.From<T>(profile, dict);
        }

        public async Task<TProperty> GetProperty<TModel, TProperty>
            (object partitionKey, object sortKey, Expression<Func<TModel, TProperty>> expression) 
            where TModel : new()
        {
            var profile = EnsureProfile<TModel>();

            var expr = expression.Body as MemberExpression;
            if (expr is null) throw new PropertyNotSelectedException();

            return await _impl.GetProperty<TProperty>(_client, profile, partitionKey, sortKey,
                expr.Member.Name).ConfigureAwait(false);
        }

        private TableProfile EnsureProfile<T>() where T : new()
        {
            var profile = _profiles.ContainsKey(typeof(T)) ? _profiles[typeof(T)]: null;
            profile ??= TypeToTableProfile.Get(typeof(T));
            return profile;
        }

        public async Task<IEnumerable<T>> GetMany<T>() where T : new()
        {
            var profile = EnsureProfile<T>();
            var listOfDicts = await _impl.GetMany<T>(_client, profile).ConfigureAwait(false);
            var list = new List<T>(listOfDicts.Count);
            foreach (var dict in listOfDicts)
            {
                var model = _conversionManager.From<T>(profile, dict);
                list.Add(model);
            }

            return list;
        }

        public Task Add<T>(T obj) where T : new()
        {
            var profile = EnsureProfile<T>();
            return _impl.Add<T>(_client, profile, obj);
        }

        public Task Remove<T>(object partitionKey, object sortKey = null) where T : new()
        {
            var profile = EnsureProfile<T>();
            return _impl.Remove<T>(_client, profile, partitionKey, sortKey);
        }

        public Task Update<T>(T obj) where T : new()
        {
            // TODO throw if non-existent
            var profile = EnsureProfile<T>();
            return _impl.Update<T>(_client, profile, obj);
        }

        public Task UpdateProperty<TModel, TProperty>(object partitionKey, object sortKey,
            Expression<Func<TModel, TProperty>> expression, TProperty value)
            where TModel : new()
        {
            // TODO throw if non-existent
            var profile = EnsureProfile<TModel>();
            
            var expr = expression.Body as MemberExpression;
            if (expr is null) throw new PropertyNotSelectedException();
            
            if (IsTryingToUpdatePrimaryKey(profile, expr.Member.Name)) throw new CannotUpdatePrimaryKeyException();

            return _impl.UpdateProperty<TProperty>(_client, profile, partitionKey, sortKey,
                expr.Member.Name, value);
        }

        private bool IsTryingToUpdatePrimaryKey(TableProfile profile, 
            string name)
        {
            string dynamoDbName = profile.PropNameToDynamoDbName.ContainsKey(name) ?
                profile.PropNameToDynamoDbName[name] : name;
            return profile.PartitionKeyName == dynamoDbName || profile.SortKeyName == dynamoDbName;
        }

        public Task AddToProperty<TModel>(object partitionKey, object sortKey, Expression<Func<TModel, int>> expression,
            int offset) where TModel : new()
        {
            var profile = EnsureProfile<TModel>();
            
            var expr = expression.Body as MemberExpression;
            if (expr is null) throw new PropertyNotSelectedException();
            
            if (IsTryingToUpdatePrimaryKey(profile, expr.Member.Name)) throw new CannotUpdatePrimaryKeyException();

            return _impl.AddOffsetToNumberAttribute<TModel>
                (_client, profile, partitionKey, sortKey, expr.Member.Name, offset.ToString());
        }

        public Task AddToProperty<TModel>(object partitionKey, object sortKey, Expression<Func<TModel, long>> expression,
            long offset) where TModel : new()
        {
            var profile = EnsureProfile<TModel>();
            
            var expr = expression.Body as MemberExpression;
            if (expr is null) throw new PropertyNotSelectedException();
            
            if (IsTryingToUpdatePrimaryKey(profile, expr.Member.Name)) throw new CannotUpdatePrimaryKeyException();

            return _impl.AddOffsetToNumberAttribute<TModel>
                (_client, profile, partitionKey, sortKey, expr.Member.Name, offset.ToString());
        }

        public Task AddToProperty<TModel>(object partitionKey, object sortKey, Expression<Func<TModel, float>> expression,
            float offset) where TModel : new()
        {
            var profile = EnsureProfile<TModel>();
            
            var expr = expression.Body as MemberExpression;
            if (expr is null) throw new PropertyNotSelectedException();
            
            if (IsTryingToUpdatePrimaryKey(profile, expr.Member.Name)) throw new CannotUpdatePrimaryKeyException();

            return _impl.AddOffsetToNumberAttribute<TModel>
                (_client, profile, partitionKey, sortKey, expr.Member.Name, offset.ToString());
        }

        public Task AddToProperty<TModel>(object partitionKey, object sortKey, Expression<Func<TModel, double>> expression,
            double offset) where TModel : new()
        {
            var profile = EnsureProfile<TModel>();
            
            var expr = expression.Body as MemberExpression;
            if (expr is null) throw new PropertyNotSelectedException();
            
            if (IsTryingToUpdatePrimaryKey(profile, expr.Member.Name)) throw new CannotUpdatePrimaryKeyException();

            return _impl.AddOffsetToNumberAttribute<TModel>
                (_client, profile, partitionKey, sortKey, expr.Member.Name, offset.ToString());
        }

        public Task AddToProperty<TModel>(object partitionKey, object sortKey, Expression<Func<TModel, decimal>> expression,
            decimal offset) where TModel : new()
        {
            var profile = EnsureProfile<TModel>();
            
            var expr = expression.Body as MemberExpression;
            if (expr is null) throw new PropertyNotSelectedException();
            
            if (IsTryingToUpdatePrimaryKey(profile, expr.Member.Name)) throw new CannotUpdatePrimaryKeyException();

            return _impl.AddOffsetToNumberAttribute<TModel>
                (_client, profile, partitionKey, sortKey, expr.Member.Name, offset.ToString());
        }
    }
}