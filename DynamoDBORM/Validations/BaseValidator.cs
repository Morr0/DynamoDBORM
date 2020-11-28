using DynamoDBORM.Exceptions;

namespace DynamoDBORM.Validations
{
    public abstract class BaseValidator
    {
        // Template method
        protected virtual void Validate<T>(T table) where T : new()
        {
            
        }

        public void ProcessValidation<T>(T table) where T : new()
        {
            Validate(table);
        }
    }
}