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

        public IRepository CreateFromEndpoint(string uri)
        {
            return Create(new AmazonDynamoDBClient(new AmazonDynamoDBConfig
            {
                ServiceURL = uri
            }));
        }

        public IRepository Create(AmazonDynamoDBClient client = null)
        {
            client ??= new AmazonDynamoDBClient();
            return new Repository(_conversionManager, client);
        }
    }
}