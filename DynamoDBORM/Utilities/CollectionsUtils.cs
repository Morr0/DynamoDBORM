using System.Collections.Generic;

namespace DynamoDBORM.Utilities
{
    public static class CollectionsUtils
    {
        public static void AddOther<T, V>(this Dictionary<T, V> current, Dictionary<T, V> other)
        {
            foreach (var item in other)
            {
                current.Add(item.Key, item.Value);
            }
        }
    }
}