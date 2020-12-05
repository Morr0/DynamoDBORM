using DynamoDBORM.Attributes;

namespace DynamoDBORMTest.ValidationTests.DummyClasses
{
    internal class UnsupportedTypeHere
    {
        [PartitionKey] public UnsupportedType Id { get; set; }
    }

    internal class UnsupportedType
    {
        
    }
}