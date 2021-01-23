using DynamoDBORM.Attributes;

namespace ConsoleTestApp
{
    public class Sample
    {
        [PartitionKey]
        public string Id { get; set; }

        public string Something { get; set; }
        
        public int NumInt { get; set; }
        public long NumLong { get; set; }
        public float NumFloat { get; set; }
        public double NumDouble{ get; set; }
        public decimal NumDecimal { get; set; }
    }
}