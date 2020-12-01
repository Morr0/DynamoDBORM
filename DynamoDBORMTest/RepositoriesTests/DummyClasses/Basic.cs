using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Attributes;

namespace DynamoDBORMTest.RepositoriesTests.DummyClasses
{
    [DynamoDBORM.Attributes.Table(PartitionKey = nameof(Id))]
    public class Basic
    {
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