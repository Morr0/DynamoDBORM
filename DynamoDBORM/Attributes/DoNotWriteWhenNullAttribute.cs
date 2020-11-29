using System;

namespace DynamoDBORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DoNotWriteWhenNullAttribute : BaseAttribute
    {
        
    }
}