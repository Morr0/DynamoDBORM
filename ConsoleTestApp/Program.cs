using System;
using System.Threading.Tasks;
using DynamoDBORM.Repositories;
using static System.Console;

namespace ConsoleTestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var repositoryFactory = new RepositoryFactory();
            var repository = repositoryFactory.Create();
            
            string id = "2021/01/";
            await repository.AddToProperty(id, null, (Sample s) => s.NumInt, 2).ConfigureAwait(false);
            await repository.AddToProperty(id, null, (Sample s) => s.NumLong, 2).ConfigureAwait(false);
            await repository.AddToProperty(id, null, (Sample s) => s.NumFloat, 2).ConfigureAwait(false);
            await repository.AddToProperty(id, null, (Sample s) => s.NumDouble, 2).ConfigureAwait(false);
            await repository.AddToProperty(id, null, (Sample s) => s.NumDecimal, 2).ConfigureAwait(false);
        }
    }
}