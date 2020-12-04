using System;
using DynamoDBORM.Converters;
using DynamoDBORM.Converters.Internals;
using DynamoDBORM.Exceptions.Converters;
using DynamoDBORMTest.ConvertersTests.DummyClasses;
using Xunit;

namespace DynamoDBORMTest.ConvertersTests
{
    public class PrimitivesConverterToDynamoTests
    {
        private ConversionManager _sut = new ConversionManager();

        [Fact]
        public void To()
        {
            Assert.Equal(ToDynamoDB.To, _sut.ToAttVal);
        }
        
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
        public void ShouldThrowUnsupportedTypeExceptionOnANonPrimitiveType()
        {
            var obj = new UnsupportedTypeExists
            {
                Id = "ff"
            };

            Action action = () => _sut.To(obj);

            Assert.Throws<UnsupportedTypeException>(action);
        }
        
        [Fact]
        public void ShouldUseSpecificAttributeNameWithAttributeNameAttribute()
        {
            var obj = new DifferentNamedProperty
            {
                Id = "k"
            };

            var attrs = _sut.To(obj);
            
            Assert.True(attrs.ContainsKey(nameof(DifferentNamedProperty.Id)));
            Assert.False(attrs.ContainsKey(nameof(DifferentNamedProperty.X)));
            Assert.True(attrs.ContainsKey(DifferentNamedProperty.SecondName));
        }
        
        [Fact]
        public void ShouldUseDifferentPartitionKeyNameWithAttributeNameAttribute()
        {
            var obj = new DifferentNamedPartitionKey
            {
                Id = "k"
            };

            var attrs = _sut.To(obj);
            
            Assert.True(attrs.ContainsKey(DifferentNamedPartitionKey.IdName));
            Assert.False(attrs.ContainsKey(nameof(DifferentNamedPartitionKey.Id)));
        }
        
        [Fact]
        public void ShouldUseDifferentPrimaryKeyNameWithAttributeNameAttribute()
        {
            var obj = new DifferentNamedPartitionAndSortKey
            {
                Partition = "parition",
                Sort = "sort"
            };

            var attrs = _sut.To(obj);
            
            Assert.True(attrs.ContainsKey(DifferentNamedPartitionAndSortKey.PartitionName));
            Assert.True(attrs.ContainsKey(DifferentNamedPartitionAndSortKey.SortName));
            Assert.False(attrs.ContainsKey(nameof(DifferentNamedPartitionAndSortKey.Partition)));
            Assert.False(attrs.ContainsKey(nameof(DifferentNamedPartitionAndSortKey.Sort)));
        }
    }
}