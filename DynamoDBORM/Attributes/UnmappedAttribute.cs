using System;

namespace DynamoDBORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UnmappedAttribute : BaseAttribute
    {
        
    }
}