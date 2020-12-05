using System;
using System.Collections.Generic;
using DynamoDBORM.Repositories;
using DynamoDBORM.Utilities;

namespace DynamoDBORM.Converters
{
    internal class TableProfiles
    {
        internal TableProfiles(){}

        public static Dictionary<Type, TableProfile> Profiles;
    }
}