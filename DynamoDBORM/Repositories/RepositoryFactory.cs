using Amazon.DynamoDBv2;
using DynamoDBORM.Converters;

namespace DynamoDBORM.Repositories
{
    public sealed class RepositoryFactory
    {
        private ConversionManager _conversionManager;

        public RepositoryFactory()
        {
            _conversionManager = new ConversionManager();
        }

        public IRepository Create()
        {
            return new Repository(_conversionManager, new AmazonDynamoDBClient());
        }
    }
}