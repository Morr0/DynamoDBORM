using DynamoDBORM.Attributes;

namespace DynamoDBORMTest.UtilitiesTest.DummyClasses
{
    [Table]
    public class FullTypeWithPropNames
    {
        [PartitionKey] public string Partition { get; set; }
        [SortKey] public string Sort { get; set; }
    }
}