﻿using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Attributes;

namespace DynamoDBORMTest.RepositoriesTests.DummyClasses
{
    public class MultipleProps
    {
        [PartitionKey]
        public string Id { get; set; }

        public string Something { get; set; }
        public int NumInt { get; set; }
        public long NumLong { get; set; }
        public float NumFloat { get; set; }
        public double NumDouble{ get; set; }
        public decimal NumDecimal { get; set; }

        public static GetItemResponse DynamoGetItemResponse(string something) => new GetItemResponse
        {
            Item = new Dictionary<string, AttributeValue>
            {
                {nameof(Something), new AttributeValue {S = something}}
            }
        };
    }
    
    public class UnmappedMultipleProps
    {
        [PartitionKey]
        public string Id { get; set; }

        [Unmapped]
        public string Something { get; set; }
        
        public static GetItemResponse DynamoGetItemResponse(string something) => new GetItemResponse
        {
            Item = new Dictionary<string, AttributeValue>
            {
                {nameof(Something), new AttributeValue {S = something}}
            }
        };
    }
}