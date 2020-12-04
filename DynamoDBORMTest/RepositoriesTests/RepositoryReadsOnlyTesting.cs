using System.Collections.Generic;
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
                    x.GetItemAsync(It.IsAny<GetItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Basic.DynamoGetItemResponse(id));

            // Act
            var response = await _sut.Get<Basic>(id);
            
            // Assert
            _dynamoDBClient.Verify(x => x.GetItemAsync(It.IsAny<GetItemRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(obj.Id, response.Id);
        }

        [Fact]
        public async Task ShouldScanItemsFromDynamoDB()
        {
            // Arrange
            string id = "2";
            Basic obj1 = new Basic
            {
                Id = id
            };
            Basic obj2 = new Basic
            {
                Id = id
            };
            _dynamoDBClient.Setup(x =>
                    x.ScanAsync(It.IsAny<ScanRequest>(), CancellationToken.None))
                .ReturnsAsync(new ScanResponse
                {
                    Items = new List<Dictionary<string, AttributeValue>>
                    {
                        Basic.DynamoGetItemResponse(obj1.Id).Item,
                        Basic.DynamoGetItemResponse(obj2.Id).Item
                    }
                });

            // Act
            var list = new List<Basic>(await _sut.GetMany<Basic>());

            // Assert
            _dynamoDBClient.Verify(x => x.ScanAsync(It.IsAny<ScanRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(obj1.Id, list[0].Id);
            Assert.Equal(obj1.Id, list[1].Id);
        }
    }
}