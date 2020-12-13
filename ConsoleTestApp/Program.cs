using System;
using DynamoDBORM.Main;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new TableContexts();
            var cm = new ContextManager(context);
            context.Sample.Add(new Sample
            {
                Id = "Hello"
            });
        }
    }
}