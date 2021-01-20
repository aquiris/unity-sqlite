using Aquiris.SQLite.Tests.Shared;
using NUnit.Framework;

namespace Aquiris.SQLite.Tests
{
    public class ConnectionTests : BaseTestClass
    {
        [Test]
        public void TestConnectionOpenClose()
        {
            CreateDatabase();
            
            SQLiteDatabase database = new SQLiteDatabase(Constants.databaseFilePath);
            OpenResult openResult = database.Open();
            Assert.AreEqual(OpenResult.Open, openResult);

            // if not closed the teardown call will fail because the db file is locked.
            CloseResult closeResult = database.Close();
            Assert.AreEqual(CloseResult.Close, closeResult);
        }

        [Test]
        public void TestConnectionOpenCloseWithOut()
        {
            CreateDatabase(out SQLiteDatabase database);
            
            OpenResult openResult = database.Open();
            Assert.AreEqual(OpenResult.Open, openResult);
            
            // if not closed the teardown call will fail because the db file is locked.
            CloseResult closeResult = database.Close();
            Assert.AreEqual(CloseResult.Close, closeResult);
        }

        private static void CreateDatabase()
        {
            CreateResult result = SQLiteDatabase.Create(Constants.databaseFilePath);
            Assert.AreEqual(CreateResult.Create, result);
        }

        private static void CreateDatabase(out SQLiteDatabase database)
        {
            CreateResult result = SQLiteDatabase.Create(Constants.databaseFilePath, out database);
            Assert.AreEqual(CreateResult.Create, result);
        }
    }
}