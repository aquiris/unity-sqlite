using System.Threading;
using Aquiris.SQLite.Runtime.Insertion;
using Aquiris.SQLite.Tables;
using Aquiris.SQLite.Tests.Shared;
using NUnit.Framework;

namespace Aquiris.SQLite.Tests
{
    public class InsertTests : BaseTestClass
    {
        private static readonly SQLiteColumn _intColumn = new SQLiteColumn("IntColumn", SQLiteDataType.Integer);
        private static readonly SQLiteColumn _floatColumn = new SQLiteColumn("FloatColumn", SQLiteDataType.Real);
        private static readonly SQLiteColumn _stringColumn = new SQLiteColumn("StringColumn", SQLiteDataType.Text);
        
        [Test]
        public void TestInsertingData()
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
            
            Assert.IsTrue(waiter.WaitOne(1000));

            SQLiteInsert insert = new SQLiteInsert(table);
            SQLiteInsertData data = new SQLiteInsertData(table);
            data.Add(_intColumn, 255);
            data.Add(_floatColumn, 3.14F);
            data.Add(_stringColumn, "This is a string");
            
            insert.Insert(data, _database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(1, result.value); // number of added rows
                waiter.Set();
            });
            
            Assert.IsTrue(waiter.WaitOne(1000));
        }

        private static SQLiteTable GetTable()
        {
            return new SQLiteTable("MyTable",new []
            {
                _intColumn,
                _floatColumn,
                _stringColumn,
            });
        }
    }
}