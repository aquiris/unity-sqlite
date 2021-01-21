using Aquiris.SQLite.Tests.Shared;
using NUnit.Framework;

namespace Aquiris.SQLite.Tests
{
    public class ConnectionTests : BaseTestClass
    {
        [Test]
        public void TestConnectionOpen()
        {
            CreateResult result = SQLiteDatabase.Create(Constants.databaseFilePath);
            Assert.AreEqual(CreateResult.Create, result);
            
            _database = new SQLiteDatabase(Constants.databaseFilePath);
            OpenResult openResult = _database.Open();
            Assert.AreEqual(OpenResult.Open, openResult);
        }

        [Test]
        public void TestConnectionOpenAutoCreate()
        {
            CreateDatabase();
            
            OpenResult openResult = _database.Open();
            Assert.AreEqual(OpenResult.Open, openResult);
        }
    }
}
