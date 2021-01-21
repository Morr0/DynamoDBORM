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
            var obj = await repository.UpdateProperty<Sample, string>(id, null,
                x => x.Something, "BBQ44").ConfigureAwait(false);
            WriteLine(obj.Something);

            await Task.Delay(1000);

            string value = await repository.GetProperty<Sample, string>(id, null, 
                x => x.Something).ConfigureAwait(false);
            WriteLine(value);
        }
    }
}