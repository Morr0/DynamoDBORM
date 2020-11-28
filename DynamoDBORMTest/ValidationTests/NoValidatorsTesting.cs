using System;
using System.Collections.Generic;
using DynamoDBORM.Exceptions;
using DynamoDBORM.Validations;
using DynamoDBORMTest.ValidationTests.DummyClasses;
using Xunit;

namespace DynamoDBORMTest.ValidationTests
{
    public class NoValidatorsTesting
    {
        private ValidationsPipeline _sut = new ValidationsPipeline(new List<BaseValidator>());

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

            Assert.Throws<NoPublicParameterlessConstructorException>(expr);
        }
    }
}