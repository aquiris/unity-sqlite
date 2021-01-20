using Aquiris.SQLite;
using Aquiris.SQLite.Tests.Shared;
using NUnit.Framework;

namespace Tests
{
    public class DatabaseCreationTests : BaseTestClass
    {
        [Test]
        public void TestCreateDatabaseSuccess()
        {
            CreateResult result = SQLiteDatabase.Create(Constants.databaseFilePath);
            Assert.AreEqual(CreateResult.Create, result);
        }

        [Test]
        public void TestAlreadyExistingDatabase()
        {
            CreateResult result = SQLiteDatabase.Create(Constants.databaseFilePath);
            Assert.AreEqual(CreateResult.Create, result);
            result = SQLiteDatabase.Create(Constants.databaseFilePath);
            Assert.AreEqual(CreateResult.AlreadyExists, result);
        }
    }
}
