using System.Threading.Tasks;
using DynamoDBORM.Repositories;

namespace ConsoleTestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var repositoryFactory = new RepositoryFactory();
            var repository = repositoryFactory.Create();
            
            var sample = new Sample
            {
                Id = "2021/01/"
            };
            await repository.Add(sample).ConfigureAwait(false);
        }
    }
}