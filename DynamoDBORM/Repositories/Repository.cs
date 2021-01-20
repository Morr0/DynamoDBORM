using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using DynamoDBORM.Converters;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Repositories
{
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

            _impl = new RepositoryImpl(_conversionManager, _client);
        }
        
        public async Task<T> Get<T>(object partitionKey, object sortKey = null) where T : new()
        {
            var profile = EnsureProfile<T>();
            var dict = await _impl.Get<T>(profile, partitionKey, sortKey).ConfigureAwait(false);
            return _conversionManager.From<T>(dict);
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
            var listOfDicts = await _impl.GetMany<T>(profile).ConfigureAwait(false);
            var list = new List<T>(listOfDicts.Count);
            foreach (var dict in listOfDicts)
            {
                var model = _conversionManager.From<T>(dict);
                list.Add(model);
            }

            return list;
        }

        public Task Add<T>(T obj) where T : new()
        {
            var profile = EnsureProfile<T>();
            return _impl.Add<T>(profile, obj);
        }

        public Task Remove<T>(object partitionKey, object sortKey = null) where T : new()
        {
            var profile = _profiles[typeof(T)];
            return _impl.Remove<T>(profile, partitionKey, sortKey);
        }

        public Task Update<T>(T obj) where T : new()
        {
            var profile = EnsureProfile<T>();
            return _impl.Update<T>(profile, obj);
        }
    }
}