using System.IO;
using Aquiris.SQLite;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class DatabaseCreationTests
    {
        private static readonly string path = Path.Combine(Application.dataPath, "SQLite");
        private static readonly string databaseFilePath = Path.Combine(path, "database.db");

        [SetUp]
        public void SetUp()
        {
            if (!File.Exists(databaseFilePath)) return;
            File.Delete(databaseFilePath);
        }
        
        [Test]
        public void TestCreateDatabaseSuccess()
        {
            CreateResult result = SQLiteDatabase.Create(databaseFilePath);
            Assert.AreEqual(CreateResult.Create, result);
        }

        [Test]
        public void TestAlreadyExistingDatabase()
        {
            CreateResult result = SQLiteDatabase.Create(databaseFilePath);
            Assert.AreEqual(CreateResult.Create, result);
            result = SQLiteDatabase.Create(databaseFilePath);
            Assert.AreEqual(CreateResult.AlreadyExists, result);
        }
    }
}
