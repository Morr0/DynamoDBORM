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
    public class RepositoryRemoveTesting
    {
        private Mock<AmazonDynamoDBClient> _dynamoDBClient = new Mock<AmazonDynamoDBClient>();
        private IRepository _sut;

        public RepositoryRemoveTesting()
        {
            _sut = new Repository(new ConversionManager(), _dynamoDBClient.Object);
        }
        
        [Fact]
        public async Task ShouldRemoveInDynamoDb()
        {
            // Arrange
            string id = "k";
            _dynamoDBClient.Setup(x =>
                    x.DeleteItemAsync(It.IsAny<DeleteItemRequest>(), CancellationToken.None))
                .ReturnsAsync(It.IsAny<DeleteItemResponse>());

            // Act
            await _sut.Remove<Basic>(id);

            // Assert
            _dynamoDBClient.Verify(x => x.DeleteItemAsync(It.IsAny<DeleteItemRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}