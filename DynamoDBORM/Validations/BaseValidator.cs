using System;
using System.Collections.Generic;
using DynamoDBORM.Attributes;

namespace DynamoDBORM.Validations
{
    public abstract class BaseValidator
    {
        // Template method
        protected abstract void Validate(ref object model,ref ISet<Type> attributes);

        public void ProcessValidation(ref object model, ref ISet<Type> attributes)
        {
            Validate(ref model, ref attributes);
        }
    }
}