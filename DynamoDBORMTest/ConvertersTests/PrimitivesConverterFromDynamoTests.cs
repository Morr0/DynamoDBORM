﻿using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using DynamoDBORM.Converters;
using DynamoDBORM.Exceptions.Converters;
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

            var obj = _sut.From<OneProp>(dict);
            
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

            var obj = _sut.From<OneProp>(dict);
        }

        [Fact]
        public void ShouldThrowWhenPartitionKeyReferencedButWasNotInDynamoDB()
        {
            var dict = new Dictionary<string, AttributeValue>
            {
                { nameof(CompositePrimaryKeyPropsOnly.PartitionKey), new AttributeValue { S = "4646"}},
                { nameof(CompositePrimaryKeyPropsOnly.SortKey), new AttributeValue{ S = "jf"}}
            };

            Action action = () => _sut.From<OneProp>(dict);

            var exception = Assert.Throws<PrimaryKeyInModelNonExistentInDynamoDBException>(action);
            Assert.Equal(ConversionExceptionReason.NonExistentPartitionKey, exception.Reason);
        }
        
        [Fact]
        public void ShouldThrowWhenDefinedSortKeyReferencedButWasNotInDynamoDB()
        {
            var dict = new Dictionary<string, AttributeValue>
            {
                { nameof(CompositePrimaryKeyPropsOnly.PartitionKey), new AttributeValue { S = "4646"}},
                { "sort", new AttributeValue{ S = "jf"}}
            };

            Action action = () => _sut.From<CompositePrimaryKeyPropsOnly>(dict);

            var exception = Assert.Throws<PrimaryKeyInModelNonExistentInDynamoDBException>(action);
            Assert.Equal(ConversionExceptionReason.NonExistentSortKey, exception.Reason);
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

            var obj = _sut.From<CompositePrimaryKeyPropsOnly>(dict);
            
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

            var obj = _sut.From<CompositePrimaryKeyPropsWithOneUnmapped>(dict);
            
            Assert.NotEqual(value, obj.Unmapped);
        }

        [Fact]
        public void ShouldThrowWhenMappingToAnUnsupportedType()
        {
            var dict = new Dictionary<string, AttributeValue>
            {
                { nameof(UnsupportedTypeExists.Id), new AttributeValue{ S = "j"}},
                { nameof(UnsupportedTypeExists.Type), new AttributeValue{ S = "jgjgj"}}
            };

            Action action = () => _sut.From<UnsupportedTypeExists>(dict);

            Assert.Throws<UnsupportedTypeException>(action);
        }
        
        
    }
}