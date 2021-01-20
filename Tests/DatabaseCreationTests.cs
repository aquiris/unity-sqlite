using System.IO;
using Aquiris.SQLite;
using Aquiris.SQLite.Tests.Shared;
using NUnit.Framework;

namespace Aquiris.SQLite.Tests
{
    public class DatabaseCreationTests : BaseTestClass
    {
        [Test]
        public void TestCreateDatabaseSuccess()
        {
            CreateResult result = SQLiteDatabase.Create(Constants.databaseFilePath);
            Assert.AreEqual(CreateResult.Create, result);
            
            File.Delete(Constants.databaseFilePath);

            result = SQLiteDatabase.Create(Constants.databaseFilePath, out _database);
            Assert.AreEqual(CreateResult.Create, result);
            Assert.IsNotNull(_database, "Database should not be null here");
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
