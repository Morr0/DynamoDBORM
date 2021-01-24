using System;

namespace DynamoDBORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PartitionKeyAttribute : AttributeNameAttribute
    {
        public PartitionKeyAttribute(string name = null) : base(name)
        {
            
        }
    }
}