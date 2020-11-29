using DynamoDBORM.Attributes;

namespace DynamoDBORMTest.ConvertersTests.DummyClasses
{
    [Table(PartitionKey = nameof(Id))]
    public class OneProp
    {
        public string Id { get; set; }
    }
    
    [Table(PartitionKey = nameof(PartitionKey), SortKey = nameof(SortKey))]
    public class CompositePrimaryKeyPropsOnly
    {
        public string PartitionKey { get; set; }
        public string SortKey { get; set; }
    }
    
    [Table(PartitionKey = nameof(PartitionKey))]
    public class CompositePrimaryKeyPropsWithOneUnmapped
    {
        public string PartitionKey { get; set; }

        [Unmapped]
        public string Unmapped { get; set; }
        
        public string Mapped { get; set; }
    }
}