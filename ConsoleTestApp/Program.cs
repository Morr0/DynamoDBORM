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
            var repository = repositoryFactory.Create();
            
            string id = "2021/01/";
            var sample = await repository.Get<Sample>("1", null).ConfigureAwait(false);
            WriteLine(sample.Decimals.Count);

        }
    }
}