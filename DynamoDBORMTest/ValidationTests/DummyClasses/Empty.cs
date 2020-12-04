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
        public string Id { get; set; }
    }

    public class TableWithBothPartitionAndSortKeysButNotDefinedInProperty
    {
        [PartitionKey]
        public string Id { get; set; }
    }

    public class MultiplePartitionKeys
    {
        [PartitionKey]
        public string Id1 { get; set; }
        [PartitionKey]
        public string Id2 { get; set; }
    }
    
    public class MultipleSortKey
    {
        [PartitionKey]
        public string Id1 { get; set; }
        [SortKey]
        public string Id2 { get; set; }
        [SortKey]
        public string Id3 { get; set; }
    }
}