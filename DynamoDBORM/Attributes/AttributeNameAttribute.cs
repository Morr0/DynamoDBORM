using System;
using System.Runtime.CompilerServices;

namespace DynamoDBORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AttributeNameAttribute : BaseAttribute
    {
        public AttributeNameAttribute(string name)
        {
            Name = name;
        }
        
        public string Name { get; set; }
    }
}