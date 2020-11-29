using System;

namespace DynamoDBORM.Attributes
{
    public abstract class BaseAttribute : Attribute
    {
        public virtual string Name { get; set; }
    }
}