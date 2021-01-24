using System;

namespace DynamoDBORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SortKeyAttribute : AttributeNameAttribute
    {
        public SortKeyAttribute(string name = null) : base(name)
        {
        }
    }
}