using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions.Repositories;
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
            _sut = new Repository(new ConversionManager(), _dynamoDBClient.Object);
        }
        
        [Fact]
        public async Task ShouldUpdateInDynamoDb()
        {
            // Arrange
            string something = "NNN";
            var obj = new MultipleProps
            {
                Id = "UU",
                Something = something
            };
            
            _dynamoDBClient.Setup(x => 
                    x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<UpdateItemResponse>());
            
            // Act
            await _sut.Update(obj);
            
            // Assert
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Once);
        }
        
        [Fact]
        public async Task ShouldUpdatePropertyInDynamoDb()
        {
            // Arrange
            string updateValue = "KKKKnwdf";
            var obj = new MultipleProps
            {
                Id = "UU",
                Something = updateValue
            };

            _dynamoDBClient.Setup(x => 
                    x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), 
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UpdateItemResponse
                {
                    Attributes = new Dictionary<string, AttributeValue>
                    {
                        {nameof(MultipleProps.Id), new AttributeValue{S = obj.Id}},
                        {nameof(MultipleProps.Something), new AttributeValue{ S = updateValue}}
                    }
                });
            
            // Act
            var obj1 = await _sut.UpdateProperty(obj.Id, null, 
                (MultipleProps x) => x.Something,
                updateValue);
            
            // Assert
            Assert.Equal(updateValue, obj1.Something);
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Once);
        }
        
        [Fact]
        public async Task ShouldUpdatePropertyForNullInDynamoDb()
        {
            // Arrange
            var obj = new MultipleProps
            {
                Id = "UU",
                Something = null
            };

            _dynamoDBClient.Setup(x => 
                    x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), 
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UpdateItemResponse
                {
                    Attributes = new Dictionary<string, AttributeValue>
                    {
                        {nameof(MultipleProps.Id), new AttributeValue{S = obj.Id}},
                        {nameof(MultipleProps.Something), new AttributeValue{ S = null}}
                    }
                });
            
            // Act
            var obj1 = await _sut.UpdateProperty(obj.Id, null, 
                (MultipleProps x) => x.Something,
                null);
            
            // Assert
            Assert.Null(obj1.Something);
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Once);
        }
        
        [Fact]
        public async Task ShouldThrowWhenUpdatingAnyPrimaryKey()
        {
            // Arrange
            var obj = new Basic
            {
                Id = "UU"
            };

            // Act
            var task = _sut.UpdateProperty(obj.Id, null, 
                (Basic x) => x.Id, null);
            
            // Assert
            await Assert.ThrowsAsync<CannotUpdatePrimaryKeyException>(async() => await task);
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Never);
        }
    }
}