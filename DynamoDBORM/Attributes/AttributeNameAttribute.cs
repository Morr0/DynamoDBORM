using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace DynamoDBORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AttributeNameAttribute : BaseAttribute
    {
        [Required]
        public override string Name { get; set; }
    }
}