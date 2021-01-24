using DynamoDBORM.Attributes;

namespace DynamoDBORMTest.UtilitiesTest.DummyClasses
{
    [Table(TableName)]
    public class FullTypeWithDiffNames
    {
        [PartitionKey(ParitionName)] public string Partition { get; set; }
        [SortKey(SortName)] public string Sort { get; set; }

        [Unmapped] public string UnmappedAttribute { get; set; }
        [AttributeName(DiffName)]
        public string Diff { get; set; }

        public const string TableName = "_table";
        public const string ParitionName = "_partition";
        public const string SortName = "_sort";
        public const string DiffName = "_diff";
    }
}