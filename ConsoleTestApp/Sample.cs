using System.Collections.Generic;
using DynamoDBORM.Attributes;

namespace ConsoleTestApp
{
    public class Sample
    {
        [PartitionKey]
        public string Id { get; set; }

        public string Something { get; set; }

        public List<int> Ints { get; set; }
        public List<long> Longs { get; set; }
        public List<float> Floats { get; set; }
        public List<double> Doubles { get; set; }
        public List<decimal> Decimals { get; set; }
    }
}