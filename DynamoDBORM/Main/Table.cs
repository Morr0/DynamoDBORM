using System.Collections.Generic;
using System.Threading.Tasks;
using DynamoDBORM.Repositories;

namespace DynamoDBORM.Main
{
    public sealed class Table<TTableModel, TTableConfiguration> 
        where TTableModel : new()
        where TTableConfiguration : TableConfiguration
    {
        private IRepository _repository;

        internal Table()
        {
            
        }

        internal void AddRepository(IRepository repository)
        {
            _repository = repository;
        }

        public Task<TTableModel> Get(object partitionKey, object sortKey = null)
        {
            return _repository.Get<TTableModel>(partitionKey, sortKey);
        }

        public Task<IEnumerable<TTableModel>> GetMany()
        {
            return _repository.GetMany<TTableModel>();
        }

        public Task Add(TTableModel obj)
        {
            return _repository.Add(obj);
        }

        public Task Remove(object partitionKey, object sortKey = null)
        {
            return _repository.Remove<TTableModel>(partitionKey, sortKey);
        }

        public Task Update(TTableModel obj)
        {
            return _repository.Update(obj);
        }
    }
}