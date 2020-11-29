using System;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions.Converters;
using DynamoDBORMTest.ConvertersTests.DummyClasses;
using Xunit;

namespace DynamoDBORMTest.ConvertersTests
{
    public class PrimitivesConverterToDynamoTests
    {
        private ConversionManager _sut = new ConversionManager();
        
        [Fact]
        public void ShouldThrowNullWhenConvertingAndPartitionKeyIsNull()
        {
            var oneProp = new OneProp();

            Action action = () => _sut.To(oneProp);

            var exception = Assert.Throws<NullPrimaryKeyException>(action);
            Assert.Equal(ConversionExceptionReason.NullPartitionKey, exception.Reason);
        }
        
        [Fact]
        public void ShouldThrowNullWhenConvertingAndTheDefinedSortKeyIsNull()
        {
            var composite = new CompositePrimaryKeyPropsOnly
            {
                PartitionKey = "Some Partition"
            };

            Action action = () => _sut.To(composite);

            var exception = Assert.Throws<NullPrimaryKeyException>(action);
            Assert.Equal(ConversionExceptionReason.NullSortKey, exception.Reason);
        }

        [Fact]
        public void ShouldPassWhenConvertingAllProps()
        {
            var composite = new CompositePrimaryKeyPropsOnly
            {
                PartitionKey = "1",
                SortKey = "AAA"
            };

            var attrs = _sut.To(composite);
            
            Assert.Equal(2, attrs.Count);
        }

        [Fact]
        public void ShouldNotComplainIfNonPrimaryKeyAttributeIsNull()
        {
            var obj = new CompositePrimaryKeyPropsWithOneUnmapped
            {
                PartitionKey = "sfn"
            };

            _sut.To(obj);
        }

        [Fact]
        public void ShouldNotIncludeUnmappedPropsWhenConverting()
        {
            var obj = new CompositePrimaryKeyPropsWithOneUnmapped
            {
                PartitionKey = "klg"
            };

            var attrs = _sut.To(obj);
            
            Assert.Equal(2, attrs.Count);
        }

        [Fact]
        public void ShouldDoNotWriteWhenNullAttributeNotApplyToPartitionKey()
        {
            var obj = new ShouldNotWriteAppliedToPartitionKey
            {
                Id = "f"
            };

            var attrs = _sut.To(obj);
            
            Assert.Single(attrs);
        }
        
        [Fact]
        public void ShouldDoNotWriteWhenNullAttributeNotApplyToADefinedSortKey()
        {
            var obj = new ShouldNotWriteAppliedToSortKey
            {
                Partition = "f",
                Sort = "fjjf"
            };

            var attrs = _sut.To(obj);
            
            Assert.Equal(2, attrs.Count);
        }

        [Fact]
        public void ShouldNotWriteANullProperty()
        {
            var obj = new ShouldNotWriteAppliedOnOneProp
            {
                Id = "f"
            };

            var attrs = _sut.To(obj);
            
            Assert.Single(attrs);
        }

        [Fact]
        public void ShouldThrowUnsupportedTypeExceptionOnANonPrimitiveType()
        {
            var obj = new UnsupportedTypeExists
            {
                Id = "ff"
            };

            Action action = () => _sut.To(obj);

            Assert.Throws<UnsupportedTypeException>(action);
        }
    }
}