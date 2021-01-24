using DynamoDBORM.Utilities;
using DynamoDBORMTest.UtilitiesTest.DummyClasses;
using Xunit;

namespace DynamoDBORMTest.UtilitiesTest
{
    public class TypeToTableTests
    {
        [Fact]
        public static void ShouldGetCorrectTableProfile()
        {
            var obj = new FullTypeWithPropNames
            {
                Partition = "1",
                Sort = "1"
            };

            var profile = TypeToTableProfile.Get(typeof(FullTypeWithPropNames));
            
            Assert.Equal(nameof(FullTypeWithPropNames), profile.TableName);
            Assert.Equal(nameof(FullTypeWithPropNames.Partition), profile.PartitionKeyName);
            Assert.Equal(nameof(FullTypeWithPropNames.Sort), profile.SortKeyName);
        }

        [Fact]
        public static void ShouldGetCorrectTableProfileWhenDifferentDynamoDbNamesInvolved()
        {
            var obj = new FullTypeWithDiffNames
            {
                Partition = "1",
                Sort = "1"
            };

            var profile = TypeToTableProfile.Get(typeof(FullTypeWithDiffNames));
            
            Assert.Equal(FullTypeWithDiffNames.TableName, profile.TableName);
            Assert.Equal(FullTypeWithDiffNames.ParitionName, profile.PartitionKeyName);
            Assert.Equal(FullTypeWithDiffNames.SortName, profile.SortKeyName);

            Assert.DoesNotContain(nameof(FullTypeWithDiffNames.UnmappedAttribute), profile.PropNameToDynamoDbName.Keys);
            Assert.DoesNotContain(nameof(FullTypeWithDiffNames.UnmappedAttribute), profile.DynamoDbNameToPropName.Values);
            
            Assert.Contains(nameof(FullTypeWithDiffNames.Diff), profile.PropNameToDynamoDbName.Keys);
            Assert.Contains(FullTypeWithDiffNames.DiffName, profile.PropNameToDynamoDbName.Values);
            Assert.Contains(nameof(FullTypeWithDiffNames.Diff), profile.DynamoDbNameToPropName.Values);
            Assert.Contains(FullTypeWithDiffNames.DiffName, profile.DynamoDbNameToPropName.Keys);
        }
    }
}