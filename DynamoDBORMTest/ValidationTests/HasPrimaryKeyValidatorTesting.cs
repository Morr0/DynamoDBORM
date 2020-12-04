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

        [Fact]
        public void PartitionKeyAttributeShouldNotBeWithAttributeNamedAttribute()
        {
            Action action = () => _sut.Validate(new[]
            {
                typeof(PartitionKeyAttributesShouldNotBeWithAttributeNamedAttribute)
            });

            Assert.Throws<PrimaryKeyShouldBeWithoutAttributeNamedAttributeException>(action);
        }

        [Fact]
        public void SortKeyShouldNotBeWithAttributeNamedAttribute()
        {
            Action action = () => _sut.Validate(new[]
            {
                typeof(PrimaryKeyAttributesShouldNotBeWithAttributeNamedAttribute)
            });

            Assert.Throws<PrimaryKeyShouldBeWithoutAttributeNamedAttributeException>(action);
        }

        [Fact]
        public void ShouldHaveSinglePartitionKey()
        {
            Action action = () => _sut.Validate(new[]
            {
                typeof(MultiplePartitionKeys)
            });

            Assert.Throws<MultiplePartitionKeysException>(action);
        }

        [Fact]
        public void ShouldHaveSingleSortKeyIfDefined()
        {
            Action action = () => _sut.Validate(new[]
            {
                typeof(MultipleSortKey)
            });

            Assert.Throws<MultipleSortKeysException>(action);
        }

    }
}