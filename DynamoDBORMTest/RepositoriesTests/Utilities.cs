using System;
using System.Collections.Generic;
using DynamoDBORM.Repositories;
using DynamoDBORM.Utilities;
using DynamoDBORMTest.RepositoriesTests.DummyClasses;

namespace DynamoDBORMTest.RepositoriesTests
{
    public class Utilities
    {
        internal static Dictionary<Type, TableProfile> Profiles => new Dictionary<Type, TableProfile>
            {
                { typeof(Basic), TypeToTableProfile.Get(typeof(Basic))}
            };
    }
}