using System.IO;
using Aquiris.SQLite;
using Aquiris.SQLite.Tests.Constants;
using NUnit.Framework;

namespace Tests
{
    public class DatabaseCreationTests
    {
        [SetUp]
        public void SetUp()
        {
            if (!File.Exists(Constants.databaseFilePath)) return;
            File.Delete(Constants.databaseFilePath);
        }
        
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
