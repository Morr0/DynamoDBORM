using System;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions.Converters;
using DynamoDBORM.Exceptions.Validations;
using DynamoDBORM.Validations;
using DynamoDBORMTest.ValidationTests.DummyClasses;
using Xunit;

namespace DynamoDBORMTest.ValidationTests
{
    public class HasOnlySupportedTypesValidatorTesting
    {
        private ConversionManager _conversionManager = new ConversionManager();
        private ValidationsPipeline _sut;

        public HasOnlySupportedTypesValidatorTesting()
        {
            _sut = new ValidationsPipeline(_conversionManager, null);
        }

        [Fact]
        public void ShouldThrowWhenUnsupportedTypeUsed()
        {
            Action action = () => _sut.Validate(new []
            {
                typeof(UnsupportedTypeHere)
            });

            Assert.Throws<UnsupportedTypeException>(action);
        }
    }
}