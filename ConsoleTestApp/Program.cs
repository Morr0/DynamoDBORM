using System;
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
            
            // var sample = new Sample
            // {
            //     Id = "2021/01/"
            // };
            var obj = await repository.Get<Sample>("2021/01/").ConfigureAwait(false);
            Console.WriteLine(obj.Something);
            obj.Something = "Hello world";
            await repository.Remove<Sample>(obj.Id);
        }
    }
}