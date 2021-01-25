using System;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Tables;
using Aquiris.SQLite.Tests.Shared;
using Mono.Data.Sqlite;
using NUnit.Framework;

namespace Aquiris.SQLite.Tests
{
    public class TablesTests : BaseTestClass
    {
        [Test]
        public void TestCreateTableSuccess()
        {
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(SQLiteErrorCode.Ok, result.errorCode);
                Assert.IsNull(result.errorMessage);
                Assert.IsNull(result.value);
            });
        }

        [Test]
        public void TestCreateTableFailure()
        {
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
            });
            
            table.Create(_database, result =>
            {
                Assert.IsFalse(result.success);
                Assert.IsNotNull(result.errorMessage);
                Assert.IsNotEmpty(result.errorMessage);
                Assert.IsNull(result.value);
            });
        }

        [Test]
        public void TestCreateTableIfNotExistsSuccess()
        {
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.CreateIfNotExists(_database, result =>
            {
                Assert.IsTrue(result.success);
            });
            
            table.CreateIfNotExists(_database, result =>
            {
                Assert.IsTrue(result.success);
            });
        }

        [Test]
        public void TestRenameTable()
        {
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
            });
            
            table.Rename("OtherTable", _database, result =>
            {
                Assert.IsTrue(result.success);
                // here we're being positive hoping that the rename has happened
                // successfully in the query execution
                Assert.AreEqual("OtherTable", table.name);
            });
        }

        [Test]
        public void TestDropTable()
        {
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
            });
            
            table.Drop(_database, result =>
            {
                Assert.IsTrue(result.success);
            });
        }

        [Test]
        public void TestDropTableFailure()
        {
            TestDropTable();
            
            SQLiteTable table = GetTable();
            table.Drop(_database, result =>
            {
                Assert.IsFalse(result.success);
                Assert.IsNotNull(result.errorMessage);
                Assert.IsNotEmpty(result.errorMessage);
            });
        }

        [Test]
        public void TestAddColumnSuccess()
        {
            CreateDatabase();
            _database.Open();
            
            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
            });
            
            SQLiteColumn column = new SQLiteColumn("Column4", DataType.Text);
            table.AddColumn(_database, column, result =>
            {
                Assert.IsTrue(result.success);
                // here we're hoping that the query execution was successful 
                Assert.AreEqual(column, table.columns[table.columns.Length - 1]);
            });
        }

        [Test]
        public void TestRenameColumnSuccess()
        {
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
            });

            SQLiteColumn column = Array.Find(table.columns, each => each.name.Equals("Column3"));
            column.Rename("Column3_4", table, _database, result =>
            {
                Assert.IsTrue(result.success);
                // here we're hoping that the query execution was successful
                Assert.AreEqual("Column3_4", column.name);
            });
        }

        private static SQLiteTable GetTable()
        {
            SQLiteColumn[] columns = {
                new SQLiteColumn("Column1", DataType.Integer),
                new SQLiteColumn("Column2", DataType.Real),
                new SQLiteColumn("Column3", DataType.Text),
            };
            return new SQLiteTable("TestTable", columns);
        }
    }
}
