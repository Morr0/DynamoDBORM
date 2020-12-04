using System;
using System.Collections.Generic;
using DynamoDBORM.Exceptions;
using DynamoDBORM.Exceptions.Validations;
using DynamoDBORM.Validations;
using DynamoDBORM.Validations.Internal;
using DynamoDBORMTest.ValidationTests.DummyClasses;
using Xunit;

namespace DynamoDBORMTest.ValidationTests
{
    public class HasPrimaryKeyValidatorTesting
    {
        private ValidationsPipeline _sut = new ValidationsPipeline(new List<BaseValidator>
        {
            new HasPrimaryKeyValidator()
        });

        [Fact]
        public void ShouldThrowDueToNonExistentPartitionKey()
        {
            Action action = () => _sut.Validate(new []
            {
                typeof(TableWithOnlyPartitionKeyButNotDefinedAsProperty)
            });

            Assert.Throws<NoPartitionKeyException>(action);
        }

        [Fact]
        public void ShouldPass()
        {
            Action action = () => _sut.Validate(new []
            {
                typeof(TableWithOnlyPartitionKey)
            });
        }

    }
}