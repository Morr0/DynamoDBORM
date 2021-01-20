using DynamoDBORM.Attributes;

namespace ConsoleTestApp
{
    public class Sample
    {
        [PartitionKey]
        public string Id { get; set; }

        public string Something { get; set; }
    }
}