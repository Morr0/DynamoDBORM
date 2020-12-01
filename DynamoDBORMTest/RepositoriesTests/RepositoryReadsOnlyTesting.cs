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
    public class RepositoryReadsOnlyTesting
    {
        private Mock<AmazonDynamoDBClient> _dynamoDBClient = new Mock<AmazonDynamoDBClient>();
        private IRepository _sut;

        public RepositoryReadsOnlyTesting()
        {
            _sut = new Repository(new ConversionManager(), _dynamoDBClient.Object, Utilities.Profiles);
        }

        [Fact]
        public async Task ShouldGetItemFromDynamoDBThatExistsThere()
        {
            // Arrange
            string id = ";";
            Basic obj = new Basic
            {
                Id = id
            };
            
            _dynamoDBClient.Setup(x => 
                    x.GetItemAsync(It.IsAny<GetItemRequest>(), CancellationToken.None))
                .ReturnsAsync(Basic.DynamoGetItemResponse(id));

            // Act
            var response = await _sut.Get<Basic>(id);
            
            // Assert
            Assert.Equal(obj.Id, response.Id);
        }

        [Fact]
        public async Task ShouldGetItemFromDynamoDBThatDoesNotExist()
        {
            
        }
    }
}