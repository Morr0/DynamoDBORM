using DynamoDBORM.Attributes;

namespace DynamoDBORMTest.ConvertersTests.DummyClasses
{
    public class OneProp
    {
        [PartitionKey]
        public string Id { get; set; }
    }
    
    public class CompositePrimaryKeyPropsOnly
    {
        [PartitionKey]
        public string PartitionKey { get; set; }
        [SortKey]
        public string SortKey { get; set; }
    }
    
    public class CompositePrimaryKeyPropsWithOneUnmapped
    {
        [PartitionKey]
        public string PartitionKey { get; set; }

        [Unmapped]
        public string Unmapped { get; set; }
        
        public string Mapped { get; set; }
    }
    
    public class UnsupportedTypeExists
    {
        [PartitionKey]
        public string Id { get; set; }

        public CompoundType Type { get; set; }
    }

    public class CompoundType
    {
        public string Name { get; set; }
    }

    public class DifferentNamedProperty
    {
        [PartitionKey]
        public string Id { get; set; }
        [AttributeName(Name = SecondName)]
        public string X { get; set; }

        public const string SecondName = "kfdn";
    }
    
    public class DifferentNamedPartitionKey
    {
        [PartitionKey(Name = IdName)]
        public string Id { get; set; }

        public const string IdName = "ID";
    }
    
    public class DifferentNamedPartitionAndSortKey
    {
        [PartitionKey(Name = PartitionName)]
        public string Partition { get; set; }
        [SortKey(Name = SortName)]
        public string Sort { get; set; }
        
        public const string PartitionName = "P1";
        public const string SortName = "S1";
    }
}