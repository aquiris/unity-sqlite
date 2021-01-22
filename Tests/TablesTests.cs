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
            CreateWaiter();
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(SQLiteErrorCode.Ok, result.errorCode);
                Assert.IsNull(result.errorMessage);
                Assert.IsNull(result.value);
                WaiterSet();
            });
            
            WaitOne();
        }

        [Test]
        public void TestCreateTableFailure()
        {
            CreateWaiter();
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();
            
            table.Create(_database, result =>
            {
                Assert.IsFalse(result.success);
                Assert.IsNotNull(result.errorMessage);
                Assert.IsNotEmpty(result.errorMessage);
                Assert.IsNull(result.value);
                WaiterSet();
            });
            
            WaitOne();
        }

        [Test]
        public void TestCreateTableIfNotExistsSuccess()
        {
            CreateWaiter();
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.CreateIfNotExists(_database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();
            
            table.CreateIfNotExists(_database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();
        }

        [Test]
        public void TestRenameTable()
        {
            CreateWaiter();
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();
            
            table.Rename("OtherTable", _database, result =>
            {
                Assert.IsTrue(result.success);
                // here we're being positive hoping that the rename has happened
                // successfully in the query execution
                Assert.AreEqual("OtherTable", table.name);
                WaiterSet();
            });
            
            WaitOne();
        }

        [Test]
        public void TestDropTable()
        {
            CreateWaiter();
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();
            
            table.Drop(_database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();
        }

        [Test]
        public void TestDropTableFailure()
        {
            TestDropTable();

            CreateWaiter();
            
            SQLiteTable table = GetTable();
            table.Drop(_database, result =>
            {
                Assert.IsFalse(result.success);
                Assert.IsNotNull(result.errorMessage);
                Assert.IsNotEmpty(result.errorMessage);
                WaiterSet();
            });

            WaitOne();
        }

        [Test]
        public void TestAddColumnSuccess()
        {
            CreateWaiter();

            CreateDatabase();
            _database.Open();
            
            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();

            SQLiteColumn column = new SQLiteColumn("Column4", DataType.Text);
            table.AddColumn(_database, column, result =>
            {
                Assert.IsTrue(result.success);
                // here we're hoping that the query execution was successful 
                Assert.AreEqual(column, table.columns[table.columns.Length - 1]);
                WaiterSet();
            });
            
            WaitOne();
        }

        [Test]
        public void TestRenameColumnSuccess()
        {
            CreateWaiter();
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();

            SQLiteColumn column = Array.Find(table.columns, each => each.name.Equals("Column3"));
            column.Rename("Column3_4", table, _database, result =>
            {
                Assert.IsTrue(result.success);
                // here we're hoping that the query execution was successful
                Assert.AreEqual("Column3_4", column.name);
                WaiterSet();
            });
            
            WaitOne();
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
