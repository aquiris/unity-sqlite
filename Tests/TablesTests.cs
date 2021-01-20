using System.Threading;
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
            AutoResetEvent waiter = new AutoResetEvent(false);
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(SQLiteErrorCode.Ok, result.errorCode);
                Assert.IsNull(result.errorMessage);
                Assert.IsNull(result.value);
                waiter.Set();
            });
            
            Assert.IsTrue(waiter.WaitOne(Constants.waitTimeOut));
        }

        [Test]
        public void TestCreateTableFailure()
        {
            AutoResetEvent waiter = new AutoResetEvent(false);
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                waiter.Set();
            });
            
            Assert.IsTrue(waiter.WaitOne(Constants.waitTimeOut));
            
            table.Create(_database, result =>
            {
                Assert.IsFalse(result.success);
                Assert.IsNotNull(result.errorMessage);
                Assert.IsNotEmpty(result.errorMessage);
                Assert.IsNull(result.value);
                waiter.Set();
            });
            
            Assert.IsTrue(waiter.WaitOne(Constants.waitTimeOut));
        }

        [Test]
        public void TestCreateTableIfNotExistsSuccess()
        {
            AutoResetEvent waiter = new AutoResetEvent(false);
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.CreateIfNotExists(_database, result =>
            {
                Assert.IsTrue(result.success);
                waiter.Set();
            });
            
            Assert.IsTrue(waiter.WaitOne(Constants.waitTimeOut));
            
            table.CreateIfNotExists(_database, result =>
            {
                Assert.IsTrue(result.success);
                waiter.Set();
            });
            
            Assert.IsTrue(waiter.WaitOne(Constants.waitTimeOut));
        }

        [Test]
        public void TestRenameTable()
        {
            AutoResetEvent waiter = new AutoResetEvent(false);
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                waiter.Set();
            });
            
            Assert.IsTrue(waiter.WaitOne(Constants.waitTimeOut));
            
            table.Rename("OtherTable", _database, result =>
            {
                Assert.IsTrue(result.success);
                // here we're being positive hoping that the rename has happened
                // successfully in the query execution
                Assert.AreEqual("OtherTable", table.name);
                waiter.Set();
            });
            
            Assert.IsTrue(waiter.WaitOne(Constants.waitTimeOut));
        }

        [Test]
        public void TestDropTable()
        {
            AutoResetEvent waiter = new AutoResetEvent(false);
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                waiter.Set();
            });
            
            Assert.IsTrue(waiter.WaitOne(Constants.waitTimeOut));
            
            table.Drop(_database, result =>
            {
                Assert.IsTrue(result.success);
                waiter.Set();
            });
            
            Assert.IsTrue(waiter.WaitOne(Constants.waitTimeOut));
            
            waiter.Close();
        }

        [Test]
        public void TestDropTableFailure()
        {
            TestDropTable();

            AutoResetEvent waiter = new AutoResetEvent(false);
            
            SQLiteTable table = GetTable();
            table.Drop(_database, result =>
            {
                Assert.IsFalse(result.success);
                Assert.IsNotNull(result.errorMessage);
                Assert.IsNotEmpty(result.errorMessage);
                waiter.Set();
            });

            Assert.IsTrue(waiter.WaitOne(Constants.waitTimeOut));
        }

        private static SQLiteTable GetTable()
        {
            SQLiteColumn[] columns = {
                new SQLiteColumn("Column1", SQLiteDataType.Integer),
                new SQLiteColumn("Column2", SQLiteDataType.Real),
                new SQLiteColumn("Column3", SQLiteDataType.Text),
            };
            return new SQLiteTable("TestTable", columns);
        }
    }
}
