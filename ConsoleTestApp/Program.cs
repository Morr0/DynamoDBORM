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

            string id = "2021/01/";
            var sample = new Sample
            {
                Id = id,
                Something = "UUU"
            };
            await repository.Add(sample).ConfigureAwait(false);

            await Task.Delay(1000);

            string value = await repository.GetProperty<Sample, string>(id, null, 
                x => x.Something).ConfigureAwait(false);
            Console.WriteLine(value);
        }
    }
}