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
            await _sut.UpdateProperty(obj.Id, null, 
                (MultipleProps x) => x.Something,
                updateValue);
            
            // Assert
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
            await _sut.UpdateProperty(obj.Id, null, 
                (MultipleProps x) => x.Something,
                null);
            
            // Assert
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Once);
        }
        
        [Fact]
        public async Task ShouldThrowWhenUpdatingAnyPrimaryKey()
        {
            await Assert.ThrowsAsync<CannotUpdatePrimaryKeyException>(async() 
                => await _sut.UpdateProperty("1", null, (Basic x) => x.Id, null));
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task ShouldUpdateIntPropertyInDynamoDb()
        {
            // Arrange
            _dynamoDBClient.Setup(x => 
                    x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), 
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<UpdateItemResponse>());
            // Act
            await _sut.AddToProperty("HHH", null, (MultipleProps x) => x.NumInt, 1);
            
            // Assert
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Once);
        }
        
        [Fact]
        public async Task ShouldUpdateLongPropertyInDynamoDb()
        {
            // Arrange
            _dynamoDBClient.Setup(x => 
                    x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), 
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<UpdateItemResponse>());
            // Act
            await _sut.AddToProperty("HHH", null, (MultipleProps x) => x.NumLong, 1);
            
            // Assert
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Once);
        }
        
        [Fact]
        public async Task ShouldUpdateFloatPropertyInDynamoDb()
        {
            // Arrange
            _dynamoDBClient.Setup(x => 
                    x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), 
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<UpdateItemResponse>());
            // Act
            await _sut.AddToProperty("HHH", null, (MultipleProps x) => x.NumFloat, 1);
            
            // Assert
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Once);
        }
        
        [Fact]
        public async Task ShouldUpdateDoublePropertyInDynamoDb()
        {
            // Arrange
            _dynamoDBClient.Setup(x => 
                    x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), 
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<UpdateItemResponse>());
            // Act
            await _sut.AddToProperty("HHH", null, (MultipleProps x) => x.NumDouble, 1);
            
            // Assert
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Once);
        }
        
        [Fact]
        public async Task ShouldUpdateDecimalPropertyInDynamoDb()
        {
            // Arrange
            _dynamoDBClient.Setup(x => 
                    x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), 
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<UpdateItemResponse>());
            // Act
            await _sut.AddToProperty("HHH", null, (MultipleProps x) => x.NumDecimal, 1);
            
            // Assert
            _dynamoDBClient.Verify(x => 
                x.UpdateItemAsync(It.IsAny<UpdateItemRequest>(), CancellationToken.None), Times.Once);
        }
    }
}