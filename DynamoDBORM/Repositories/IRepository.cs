using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDBORM.Repositories
{
    public interface IRepository
    {
        Task<T> Get<T>(object partitionKey, object sortKey = null) where T : new();
        Task<IEnumerable<T>> GetMany<T>() where T : new();

        Task Add<T>(T obj) where T : new();
    }
}