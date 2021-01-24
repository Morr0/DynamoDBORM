using DynamoDBORM.Attributes;

namespace DynamoDBORMTest.ValidationTests.DummyClasses
{
    public class NonEmpty
    {
        public NonEmpty(string @string)
        {
            
        }
    }
    
    public class PartitionKeyAttributesShouldNotBeWithAttributeNamedAttribute
    {
        [PartitionKey] 
        public string PartitionKey { get; set; }
    }

    public class PrimaryKeyAttributesShouldNotBeWithAttributeNamedAttribute
    {
        [PartitionKey] 
        public string PartitionKey { get; set; }
        [SortKey] 
        public string SortKey { get; set; }
    }
}