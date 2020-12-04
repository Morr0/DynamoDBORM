using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Attributes;

namespace DynamoDBORMTest.RepositoriesTests.DummyClasses
{
    public class Basic
    {
        [PartitionKey]
        public string Id { get; set; }
        
        public static GetItemResponse DynamoGetItemResponse(string id) => new GetItemResponse
        {
            Item = new Dictionary<string, AttributeValue>
            {
                { nameof(Id), new AttributeValue{ S = id}}
            }
        };
    }
}