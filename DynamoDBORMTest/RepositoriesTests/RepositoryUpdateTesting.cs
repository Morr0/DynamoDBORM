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
    public class RepositoryUpdateTesting
    {
        private Mock<AmazonDynamoDBClient> _dynamoDBClient = new Mock<AmazonDynamoDBClient>();
        private IRepository _sut;

        public RepositoryUpdateTesting()
        {
            _sut = new Repository(new ConversionManager(), _dynamoDBClient.Object, Utilities.Profiles);
        }
        
        [Fact]
        public async Task ShouldUpdateInDynamoDB()
        {
            // Arrange
            _dynamoDBClient.Setup(x => 
                    x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None))
                .ReturnsAsync(new UpdateItemResponse());
            
            // Act
            await _sut.Update(new Basic
            {
                Id = ";"
            });
            
            // Assert
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Once);
        }
    }
}