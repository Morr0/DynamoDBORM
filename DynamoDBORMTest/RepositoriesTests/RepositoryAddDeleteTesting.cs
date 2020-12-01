using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Converters;
using DynamoDBORM.Repositories;
using DynamoDBORMTest.RepositoriesTests.DummyClasses;
using Moq;
using Xunit;

namespace DynamoDBORMTest.RepositoriesTests
{
    public class RepositoryAddDeleteTesting
    {
        private Mock<AmazonDynamoDBClient> _dynamoDBClient = new Mock<AmazonDynamoDBClient>();
        private IRepository _sut;

        public RepositoryAddDeleteTesting()
        {
            _sut = new Repository(new ConversionManager(), _dynamoDBClient.Object, Utilities.Profiles);
        }

        [Fact]
        public async Task ShouldAddToDynamoDB()
        {
            // Arrange
            _dynamoDBClient.Setup(x =>
                    x.PutItemAsync(It.IsAny<PutItemRequest>(), CancellationToken.None))
                .ReturnsAsync(new PutItemResponse());

            // Act
            await _sut.Add(new Basic
            {
                Id = "kfk"
            });

            // Assert: OK if reached here
        }

        [Fact]
        public async Task ShouldDeleteInDynamoDB()
        {
            // Arrange
            string id = "k";
            _dynamoDBClient.Setup(x =>
                    x.DeleteItemAsync(It.IsAny<DeleteItemRequest>(), CancellationToken.None))
                .ReturnsAsync(It.IsAny<DeleteItemResponse>());

            // Act
            await _sut.Remove<Basic>(id);

            // Assert: OK if reached here
        }
    }
}