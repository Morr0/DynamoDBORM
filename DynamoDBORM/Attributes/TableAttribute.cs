using System;

namespace DynamoDBORM.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class TableAttribute : BaseAttribute
    {
        /// <summary>
        /// Is required as per DynamoDB specifications. Name of property.
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// Optional. Name of sort key.
        /// </summary>
        public string SortKey { get; set; }
    }
}