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

        protected bool HasPropertyNamed<T>(T table, string propName) where T : new()
        {
            var props = table.GetType().GetProperties();
            bool hasPropWithThatName = false;
            
            foreach (var prop in props)
            {
                if (prop.Name == propName)
                {
                    hasPropWithThatName = true;
                    break;
                }
            }

            return hasPropWithThatName;
        }
    }
}