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
        [AttributeName]
        [PartitionKey] 
        public string PartitionKey { get; set; }
    }

    public class PrimaryKeyAttributesShouldNotBeWithAttributeNamedAttribute
    {
        [PartitionKey] 
        public string PartitionKey { get; set; }
        [AttributeName]
        [SortKey] 
        public string SortKey { get; set; }
    }
}