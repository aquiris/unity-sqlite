using System.IO;
using NUnit.Framework;

namespace Aquiris.SQLite.Tests.Shared
{
    public abstract class BaseTestClass
    {
        protected static SQLiteDatabase _database = default;

        [SetUp]
        public virtual void SetUp()
        {
            if (!File.Exists(Constants.databaseFilePath)) return;
            File.Delete(Constants.databaseFilePath);
        }

        [TearDown]
        public virtual void TearDown()
        {
            CheckIfAlreadyClosed();
            
            if (!Directory.Exists(Constants.databaseParentPath)) return;
            Directory.Delete(Constants.databaseParentPath, true);
        }

        protected static void CreateDatabase()
        {
            CreateResult result = SQLiteDatabase.Create(Constants.databaseFilePath, out _database);
            Assert.AreEqual(CreateResult.Create, result);
        }

        private static void CheckIfAlreadyClosed()
        {
            if (_database == null) return;
            CloseResult result = _database.Close();
            Assert.IsTrue(result == CloseResult.Close || result == CloseResult.AlreadyClose);
            _database = null;
        }
    }
}
