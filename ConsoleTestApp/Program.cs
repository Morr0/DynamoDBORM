using System;
using System.Collections.Generic;
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
            var repository = repositoryFactory.CreateFromEndpoint("http://localhost:8000");

            string id = "n";
            await repository.Add(new Sample
            {
                Id = id,
                Something = "77"
            });

            WriteLine(await repository.GetProperty(id, null, (Sample s) => s.Something));


        }
    }
}