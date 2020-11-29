using System;

namespace DynamoDBORM.Attributes
{
    public abstract class BaseAttribute : Attribute
    {
        public string Name { get; set; }
    }
}