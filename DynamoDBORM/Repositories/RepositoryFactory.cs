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

        public IRepository Create(AmazonDynamoDBClient client = null)
        {
            client ??= new AmazonDynamoDBClient();
            return new Repository(_conversionManager, client);
        }
    }
}