using System;
using DynamoDBORM.Converters;
using DynamoDBORM.Converters.Internals;
using DynamoDBORM.Exceptions.Converters;
using DynamoDBORM.Utilities;
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
            var profile = TypeToTableProfile.Get(typeof(OneProp));
            Action action = () => _sut.To(profile, oneProp);

            Assert.Throws<NullPartitionKeyException>(action);
        }
        
        [Fact]
        public void ShouldThrowNullWhenConvertingAndTheDefinedSortKeyIsNull()
        {
            var composite = new CompositePrimaryKeyPropsOnly
            {
                PartitionKey = "Some Partition"
            };
            var profile = TypeToTableProfile.Get(typeof(CompositePrimaryKeyPropsOnly));
            Action action = () => _sut.To(profile, composite);

            Assert.Throws<NullSortKeyException>(action);
        }

        [Fact]
        public void ShouldPassWhenConvertingAllProps()
        {
            var composite = new CompositePrimaryKeyPropsOnly
            {
                PartitionKey = "1",
                SortKey = "AAA"
            };
            var profile = TypeToTableProfile.Get(typeof(CompositePrimaryKeyPropsOnly));
            var attrs = _sut.To(profile, composite);
            
            Assert.Equal(2, attrs.Count);
        }

        [Fact]
        public void ShouldNotComplainIfNonPrimaryKeyAttributeIsNull()
        {
            var obj = new CompositePrimaryKeyPropsWithOneUnmapped
            {
                PartitionKey = "sfn"
            };
            var profile = TypeToTableProfile.Get(typeof(CompositePrimaryKeyPropsWithOneUnmapped));
            _sut.To(profile, obj);
        }

        [Fact]
        public void ShouldNotIncludeUnmappedPropsWhenConverting()
        {
            var obj = new CompositePrimaryKeyPropsWithOneUnmapped
            {
                PartitionKey = "klg"
            };
            var profile = TypeToTableProfile.Get(typeof(CompositePrimaryKeyPropsWithOneUnmapped));
            var attrs = _sut.To(profile, obj);
            
            Assert.False(attrs.ContainsKey(nameof(CompositePrimaryKeyPropsWithOneUnmapped.Unmapped)));
        }

        [Fact]
        public void ShouldUseSpecificAttributeNameWithAttributeNameAttribute()
        {
            var obj = new DifferentNamedProperty
            {
                Id = "k"
            };
            var profile = TypeToTableProfile.Get(typeof(DifferentNamedProperty));
            var attrs = _sut.To(profile, obj);
            
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
            var profile = TypeToTableProfile.Get(typeof(DifferentNamedPartitionKey));
            var attrs = _sut.To(profile, obj);
            
            Assert.True(attrs.ContainsKey(DifferentNamedPartitionKey.IdName));
            Assert.False(attrs.ContainsKey(nameof(DifferentNamedPartitionKey.Id)));
        }
        
        [Fact]
        public void ShouldUseDifferentPrimaryKeyNameWithAttributeNameAttribute()
        {
            var obj = new DifferentNamedPartitionAndSortKey
            {
                Partition = DifferentNamedPartitionAndSortKey.PartitionName,
                Sort = DifferentNamedPartitionAndSortKey.SortName
            };
            var profile = TypeToTableProfile.Get(typeof(DifferentNamedPartitionAndSortKey));
            var attrs = _sut.To(profile, obj);
            
            Assert.True(attrs.ContainsKey(DifferentNamedPartitionAndSortKey.PartitionName));
            Assert.True(attrs.ContainsKey(DifferentNamedPartitionAndSortKey.SortName));
            Assert.False(attrs.ContainsKey(nameof(DifferentNamedPartitionAndSortKey.Partition)));
            Assert.False(attrs.ContainsKey(nameof(DifferentNamedPartitionAndSortKey.Sort)));
        }

        [Fact]
        public void ConvertFromGuidToStringInDynamoDB()
        {
            var obj = new GuidProp();
            var profile = TypeToTableProfile.Get(typeof(GuidProp));
            var attrs = _sut.To(profile, obj);
            
            Assert.Equal(obj.Id.ToString(), attrs[nameof(GuidProp.Id)].S);
        }
    }
}