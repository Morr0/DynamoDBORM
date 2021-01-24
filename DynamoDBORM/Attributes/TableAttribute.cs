using System;

namespace DynamoDBORM.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : AttributeNameAttribute
    {
        public TableAttribute(string name = null) : base(name)
        {
        }
    }
}