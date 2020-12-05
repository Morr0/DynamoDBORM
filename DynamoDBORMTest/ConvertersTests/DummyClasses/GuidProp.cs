using System;
using DynamoDBORM.Attributes;

namespace DynamoDBORMTest.ConvertersTests.DummyClasses
{
    public class GuidProp
    {
        [PartitionKey] public Guid Id { get; set; }
    }
}