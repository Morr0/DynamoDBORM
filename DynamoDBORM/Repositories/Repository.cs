using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using DynamoDBORM.Attributes;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions;

namespace DynamoDBORM.Repositories
{
    public class Repository : IRepository
    {
        private readonly ConversionManager _conversionManager;
        private readonly AmazonDynamoDBClient _client;
        private Dictionary<Type, TableProfile> _profiles;
        private readonly RepositoryImpl _impl;

        internal Repository(ConversionManager conversionManager, AmazonDynamoDBClient client, 
            Dictionary<Type, TableProfile> profiles)
        {
            _conversionManager = conversionManager;
            _client = client;
            _profiles = profiles;

            _impl = new RepositoryImpl(_conversionManager, _client);
        }
        
        public async Task<T> Get<T>(object partitionKey, object sortKey = null) where T : new()
        {
            var profile = ThrowIfTypeNotHere<T>();

            var dict = await _impl.Get<T>(profile, partitionKey, sortKey).ConfigureAwait(false);
            return _conversionManager.From<T>(dict);
        }

        private TableProfile ThrowIfTypeNotHere<T>() where T : new()
        {
            if (!_profiles.ContainsKey(typeof(T))) throw new TypeWasNotDeclaredException();

            return _profiles[typeof(T)];
        }
    }
}