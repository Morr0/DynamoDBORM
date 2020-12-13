using System;
using System.Collections.Generic;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions;
using DynamoDBORM.Exceptions.Validations;
using DynamoDBORM.Validations;
using DynamoDBORMTest.ValidationTests.DummyClasses;
using Xunit;

namespace DynamoDBORMTest.ValidationTests
{
    public class NoValidatorsTesting
    {
        private ValidationsPipeline _sut = new ValidationsPipeline(new ConversionManager(), new List<BaseValidator>());

        [Fact]
        public void ShouldPassOnEmptyClassAndStruct()
        {
            var empty1 = typeof(AbsoluteEmptyClass);
            
            _sut.Validate(new List<Type>
            {
                empty1,
            });
        }

        [Fact]
        public void ShouldThrowWhenNonEmptyConstructorsInvolved()
        {
            Action expr = () => _sut.Validate(new []
            {
                typeof(NonEmpty)
            });

            Assert.Throws<MissingMethodException>(expr);
        }
        
        [Fact]
        public void ShouldThrowWhenUnsupportedTypeUsed()
        {
            Action action = () => _sut.Validate(new []
            {
                typeof(UnsupportedTypeHere)
            });

            var exception = Assert.Throws<UnsupportedTypeException>(action);
            Assert.Equal(typeof(UnsupportedType), exception.Type);
        }
    }
}