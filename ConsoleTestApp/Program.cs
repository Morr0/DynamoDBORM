using System;
using DynamoDBORM.Main;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new TableContexts();
            DbOrm main = new DbOrm(new []
            {
                context, 
            }, null, null);

            if (context.Sample is null) Console.WriteLine("Null");
            context.Sample.Add(new Sample
            {
                Id = "Hello"
            });
        }
    }
}