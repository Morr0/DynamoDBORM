using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Repositories
{
    public interface IRepository
    {
        Task<T> Get<T>(object partitionKey, object sortKey = null) where T : new();
        Task<TProperty> GetProperty<TModel, TProperty>
            (object partitionKey, object sortKey, Expression<Func<TModel, TProperty>> prop)
            where TModel : new();
        Task<IEnumerable<T>> GetMany<T>() where T : new();
        
        
        Task Add<T>(T obj) where T : new();
        Task Remove<T>(object partitionKey, object sortKey = null) where T : new();
        Task Update<T>(T obj) where T : new();

        Task<TModel> UpdateProperty<TModel, TProperty>(object partitionKey, object sortKey,
            Expression<Func<TModel, TProperty>> expr, TProperty value)
            where TModel : new();
    }
}