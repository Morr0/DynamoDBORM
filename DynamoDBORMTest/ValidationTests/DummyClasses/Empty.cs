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

    [Table(PartitionKey = nameof(TableWithOnlyPartitionKey.Id))]
    public class TableWithOnlyPartitionKey
    {
        public string Id { get; set; }
    }
    
    [Table(PartitionKey = "hello")]
    public class TableWithOnlyPartitionKeyButNotDefinedAsProperty
    {
        public string Id { get; set; }
    }

    [Table(PartitionKey = nameof(TableWithBothPartitionAndSortKeysButNotDefinedInProperty.Id)
        , SortKey = "jjj")]
    public class TableWithBothPartitionAndSortKeysButNotDefinedInProperty
    {
        public string Id { get; set; }
    }
}