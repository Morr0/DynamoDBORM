﻿using System.Threading;
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
    public class RepositoryAddTesting
    {
        private Mock<AmazonDynamoDBClient> _dynamoDBClient = new Mock<AmazonDynamoDBClient>();
        private IRepository _sut;

        public RepositoryAddTesting()
        {
            _sut = new Repository(new ConversionManager(), _dynamoDBClient.Object);
        }

        [Fact]
        public async Task ShouldAddToDynamoDb()
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

            // Assert
            _dynamoDBClient.Verify(x 
                => x.PutItemAsync(It.IsAny<PutItemRequest>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}