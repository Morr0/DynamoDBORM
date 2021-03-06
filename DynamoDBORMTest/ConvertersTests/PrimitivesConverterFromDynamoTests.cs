﻿using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Converters;
using DynamoDBORM.Converters.Internals;
using DynamoDBORM.Exceptions.Converters;
using DynamoDBORM.Exceptions.Validations;
using DynamoDBORM.Utilities;
using DynamoDBORMTest.ConvertersTests.DummyClasses;
using Xunit;

namespace DynamoDBORMTest.ConvertersTests
{
    public class PrimitivesConverterFromDynamoTests
    {
        private ConversionManager _sut = new ConversionManager();

        [Fact]
        public void FromDynamoDBToSimpleTableModel()
        {
            string value = "S";
            var dict = new Dictionary<string, AttributeValue>
            {
                { nameof(OneProp.Id), new AttributeValue { S = value}}
            };
            var profile = TypeToTableProfile.Get(typeof(OneProp));
            var obj = _sut.From<OneProp>(profile, dict);
            
            Assert.Equal(value, obj.Id);
        }
        
        [Fact]
        public void FromDynamoDBToSimpleTableModelWithNonExistentProp()
        {
            string value = "S";
            string nonExistPropName = "prop";
            var dict = new Dictionary<string, AttributeValue>
            {
                { nameof(OneProp.Id), new AttributeValue { S = value}},
                { nameof(nonExistPropName), new AttributeValue{ S = "jf"}}
            };
            var profile = TypeToTableProfile.Get(typeof(OneProp));
            var obj = _sut.From<OneProp>(profile, dict);
        }

        [Fact]
        public void ShouldThrowWhenPartitionKeyReferencedButWasNotInDynamoDB()
        {
            var dict = new Dictionary<string, AttributeValue>
            {
                { nameof(CompositePrimaryKeyPropsOnly.PartitionKey), new AttributeValue { S = "4646"}},
                { nameof(CompositePrimaryKeyPropsOnly.SortKey), new AttributeValue{ S = "jf"}}
            };
            var profile = TypeToTableProfile.Get(typeof(OneProp));
            Action action = () => _sut.From<OneProp>(profile, dict);

            Assert.Throws<PrimaryKeyInModelNonExistentInDynamoDBException>(action);
        }
        
        [Fact]
        public void ShouldThrowWhenDefinedSortKeyReferencedButWasNotInDynamoDB()
        {
            var dict = new Dictionary<string, AttributeValue>
            {
                { nameof(CompositePrimaryKeyPropsOnly.PartitionKey), new AttributeValue { S = "4646"}},
                { "k", new AttributeValue{ S = "jf"}}
            };
            var profile = TypeToTableProfile.Get(typeof(CompositePrimaryKeyPropsOnly));
            
            Action action = () => _sut.From<CompositePrimaryKeyPropsOnly>(profile, dict);
            Assert.Throws<PrimaryKeyInModelNonExistentInDynamoDBException>(action);
        }

        [Fact]
        public void MapsAll()
        {
            string partition = "S";
            string sort = "h";
            var dict = new Dictionary<string, AttributeValue>
            {
                { nameof(CompositePrimaryKeyPropsOnly.PartitionKey), new AttributeValue { S = partition}},
                { nameof(CompositePrimaryKeyPropsOnly.SortKey), new AttributeValue{ S = sort}}
            };
            var profile = TypeToTableProfile.Get(typeof(CompositePrimaryKeyPropsOnly));
            var obj = _sut.From<CompositePrimaryKeyPropsOnly>(profile, dict);
            
            Assert.Equal(partition, obj.PartitionKey);
            Assert.Equal(sort, obj.SortKey);
        }

        [Fact]
        public void MapsAllExceptOnesMarkedWithUnmappedEvenThoughTheyExistFromDynamoDB()
        {
            string value = "0003d";
            var dict = new Dictionary<string, AttributeValue>
            {
                { nameof(CompositePrimaryKeyPropsWithOneUnmapped.PartitionKey), new AttributeValue { S = ""}},
                { nameof(CompositePrimaryKeyPropsWithOneUnmapped.Unmapped), new AttributeValue{ S = value}}
            };
            var profile = TypeToTableProfile.Get(typeof(CompositePrimaryKeyPropsWithOneUnmapped));
            var obj = _sut.From<CompositePrimaryKeyPropsWithOneUnmapped>(profile, dict);
            
            Assert.NotEqual(value, obj.Unmapped);
        }

        [Fact]
        public void ShouldUseSpecificAttributeNameWithAttributeNameAttribute()
        {
            string value = "22";
            var dict = new Dictionary<string, AttributeValue>
            {
                { nameof(DifferentNamedProperty.Id), new AttributeValue{ S = "kg"}},
                { DifferentNamedProperty.SecondName, new AttributeValue{ S = value}}
            };
            var profile = TypeToTableProfile.Get(typeof(DifferentNamedProperty));
            var obj = _sut.From<DifferentNamedProperty>(profile, dict);

            Assert.Equal(value, obj.X);
        }
        
        [Fact]
        public void ShouldUseDifferentPartitionKeyNameWithAttributeNameAttribute()
        {
            string value = "22";
            var dict = new Dictionary<string, AttributeValue>
            {
                { DifferentNamedPartitionKey.IdName, new AttributeValue{ S = value}}
            };
            var profile = TypeToTableProfile.Get(typeof(DifferentNamedPartitionKey));
            var obj = _sut.From<DifferentNamedPartitionKey>(profile, dict);

            Assert.Equal(value, obj.Id);
        }
        
        [Fact]
        public void ShouldUseDifferentPrimaryKeyNameWithAttributeNameAttribute()
        {
            string value = "22";
            var dict = new Dictionary<string, AttributeValue>
            {
                { DifferentNamedPartitionAndSortKey.PartitionName, new AttributeValue{ S = value}},
                { DifferentNamedPartitionAndSortKey.SortName, new AttributeValue{ S = value}},
            };
            var profile = TypeToTableProfile.Get(typeof(DifferentNamedPartitionAndSortKey));
            var obj = _sut.From<DifferentNamedPartitionAndSortKey>(profile, dict);

            Assert.Equal(value, obj.Partition);
            Assert.Equal(value, obj.Sort);
        }

        [Fact]
        public void ShouldConvertStringToGuidFromDynamoDB()
        {
            var id = Guid.NewGuid().ToString();
            var dict = new Dictionary<string, AttributeValue>
            {
                { nameof(GuidProp.Id), new AttributeValue{ S = id}}
            };
            var profile = TypeToTableProfile.Get(typeof(GuidProp));
            var obj = _sut.From<GuidProp>(profile, dict);
            
            Assert.Equal(id, obj.Id.ToString());
        }
    }
}