using DynamoDBORM.Attributes;

namespace DynamoDBORMTest.ValidationTests.DummyClasses
{
    public class AbsoluteEmptyClass
    {
        
    }
    
    [Table]
    public class EmptyTable
    {
        
    }
    
    public class TableWithOnlyPartitionKey
    {
        [PartitionKey]
        public string Id { get; set; }
    }
    public class TableWithOnlyPartitionKeyButNotDefinedAsProperty
    {
        [PartitionKey(Name = "hello")]
        public string Id { get; set; }
    }

    public class TableWithBothPartitionAndSortKeysButNotDefinedInProperty
    {
        [PartitionKey]
        public string Id { get; set; }
    }
}