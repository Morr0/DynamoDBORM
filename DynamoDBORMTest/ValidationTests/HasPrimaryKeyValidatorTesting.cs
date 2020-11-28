using System;
using System.Collections.Generic;
using DynamoDBORM.Exceptions;
using DynamoDBORM.Validations;
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
        public void ShouldThrowOnClassWithoutTableAttribute()
        {
            Action action = () => _sut.Validate(new []
            {
                typeof(AbsoluteEmptyClass)
            });

            Assert.Throws<NoTableAttributeException>(action);
        }
        
        [Fact]
        public void ShouldThrowOnEmptyClassWithTableAttribute()
        {
            Action action = () => _sut.Validate(new []
            {
                typeof(EmptyTable)
            });

            var exception = Assert.Throws<TableHasNotSpecifiedPartitionKey>(action);
            Assert.Equal(Reason.SameAsExceptionName, exception.Reason);
        }

        [Fact]
        public void ShouldThrowDueToNonExistentPartitionKey()
        {
            Action action = () => _sut.Validate(new []
            {
                typeof(TableWithOnlyPartitionKeyButNotDefinedAsProperty)
            });

            var exception = Assert.Throws<TableHasNotSpecifiedPartitionKey>(action);
            Assert.Equal(Reason.ReferencedNonExistentPartitionKey, exception.Reason);
        }
        
        [Fact]
        public void ShouldThrowDueToNonExistentSortKey()
        {
            Action action = () => _sut.Validate(new []
            {
                typeof(TableWithBothPartitionAndSortKeysButNotDefinedInProperty)
            });

            var exception = Assert.Throws<TableHasNotSpecifiedPartitionKey>(action);
            Assert.Equal(Reason.ReferencedNonExistentSortKey, exception.Reason);
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