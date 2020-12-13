using DynamoDBORM.Main;

namespace ConsoleTestApp
{
    public class TableContexts : DataContext
    {
        public Table<Sample, TableConfiguration> Sample { get; set; }
    }
}