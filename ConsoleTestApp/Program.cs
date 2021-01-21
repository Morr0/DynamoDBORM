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
            string preUpdate = await repository.GetProperty(id, null,
                (Sample x) => x.Something).ConfigureAwait(false);
            WriteLine(string.IsNullOrEmpty(preUpdate));
            var obj = await repository.UpdateProperty<Sample, string>(id, null,
                x => x.Something, "Pepsi").ConfigureAwait(false);
            WriteLine(obj.Something);
        }
    }
}