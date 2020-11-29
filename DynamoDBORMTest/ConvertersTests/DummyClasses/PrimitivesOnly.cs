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

    [Table(PartitionKey = nameof(Id))]
    public class ShouldNotWriteAppliedToPartitionKey
    {
        [DoNotWriteWhenNull]
        public string Id { get; set; }
    }

    [Table(PartitionKey = nameof(Partition), SortKey = nameof(Sort))]
    public class ShouldNotWriteAppliedToSortKey
    {
        public string Partition { get; set; }
        [DoNotWriteWhenNull]
        public string Sort { get; set; }
    }

    [Table(PartitionKey = nameof(Id))]
    public class ShouldNotWriteAppliedOnOneProp
    {
        public string Id { get; set; }

        [DoNotWriteWhenNull]
        public string Null { get; set; }
    }

    [Table(PartitionKey = nameof(Id))]
    public class UnsupportedTypeExists
    {
        public string Id { get; set; }

        public CompoundType Type { get; set; }
    }

    public class CompoundType
    {
        public string Name { get; set; }
    }

    [Table(PartitionKey = nameof(Id))]
    public class DifferentNamedProperty
    {
        public string Id { get; set; }
        [AttributeName(Name = SecondName)]
        public int X { get; set; }

        public const string SecondName = "kfdn";
    }
    
    [Table(PartitionKey = nameof(Id))]
    public class DifferentNamedPartitionKey
    {
        [AttributeName(Name = IdName)]
        public string Id { get; set; }

        public const string IdName = "ID";
    }
    
    [Table(PartitionKey = nameof(Partition), SortKey = nameof(Sort))]
    public class DifferentNamedPartitionAndSortKey
    {
        [AttributeName(Name = PartitionName)]
        public string Partition { get; set; }
        [AttributeName(Name = SortName)]
        public string Sort { get; set; }
        
        public const string PartitionName = "P1";
        public const string SortName = "S1";
    }
}