using Aquiris.SQLite.Queries;
using Aquiris.SQLite.Runtime.Insertion;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Tests.Shared;
using NUnit.Framework;
using SQLiteColumn = Aquiris.SQLite.Tables.SQLiteColumn;
using SQLiteInsert = Aquiris.SQLite.Runtime.Insertion.SQLiteInsert;

namespace Aquiris.SQLite.Tests
{
    public class InsertTests : BaseTestClass
    {
        private static readonly SQLiteColumn _intColumn = new SQLiteColumn("IntColumn", DataType.Integer);
        private static readonly SQLiteColumn _floatColumn = new SQLiteColumn("FloatColumn", DataType.Real);
        private static readonly SQLiteColumn _stringColumn = new SQLiteColumn("StringColumn", DataType.Text);
        
        [Test]
        public void TestInsertingData()
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

            SQLiteInsert insert = new SQLiteInsert(table);
            
            SQLiteInsertData data = new SQLiteInsertData(table);
            data.Add(_intColumn, 255);
            data.Add(_floatColumn, 3.14F);
            data.Add(_stringColumn, "This is a string");
            insert.Insert(InsertMode.insert, data, _database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(1, result.value); // number of added rows
                WaiterSet();
            });
            
            WaitOne();
        }

        [Test]
        public void TestInsertingBatchData()
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

            SQLiteInsert insert = new SQLiteInsert(table);

            const int itemCount = 10000;
            SQLiteInsertData[] collection = new SQLiteInsertData[itemCount];
            for (int index = 0; index < collection.Length; index++)
            {
                SQLiteInsertData data = new SQLiteInsertData(table);
                data.Add(_intColumn, 255 * index);
                data.Add(_floatColumn, 3.14F * index);
                data.Add(_stringColumn, $"Value of {index}");
                collection[index] = data;
            }
            
            insert.Insert(InsertMode.insert, collection, _database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(itemCount, result.value);
                WaiterSet();
            });
            
            WaitOne();
        }

        [Test]
        public void TestInsertOrAbort()
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

            SQLiteInsert insert = new SQLiteInsert(table);
            SQLiteInsertData data = new SQLiteInsertData(table);
            data.Add(_intColumn, 10);
            insert.Insert(InsertMode.insert, data, _database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();
            
            insert.Insert(InsertMode.insertOrAbort, data, _database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();
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