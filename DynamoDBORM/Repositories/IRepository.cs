using System.Threading.Tasks;

namespace DynamoDBORM.Repositories
{
    public interface IRepository
    {
        Task<T> Get<T>(object partitionKey, object sortKey = null) where T : new();
    }
}