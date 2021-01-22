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
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
            });
            
            Query query = CreateInsert(new Insert(), 1).Build();
            SQLiteInsert.Run(query, _database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(1, result.value); // number of added rows
            });
        }

        [Test]
        public void TestInsertingBatchData()
        {
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
            });
            
            const int itemCount = 1000;
            Insert insert = new Insert();
            for (int index = 0; index < itemCount; index++)
            {
                insert = CreateInsert(insert, index);
            }
            SQLiteInsert.Run(insert.Build(), _database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(itemCount, result.value);
            });
        }

        [Test]
        public void TestInsertOrAbort()
        {
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
            });
            
            Query query = CreateInsert(new Insert(), 10).Build();
            
            SQLiteInsert.Run(query, _database, result =>
            {
                Assert.IsTrue(result.success);
            });

            query = CreateInsert(new Insert(), 10, InsertMode.insertOrAbort)
                .Build();
            
            SQLiteInsert.Run(query, _database, result =>
            {
                Assert.IsTrue(result.success);
            });
        }

        private static SQLiteTable GetTable()
        {
            return new SQLiteTable("TestTable",new []
            {
                _intColumn,
                _floatColumn,
                _stringColumn,
            });
        }
        
        private static Insert CreateInsert(Insert insert, int index, InsertMode mode = InsertMode.insert)
        {
            return insert.Begin(mode)
                .IntoTable("TestTable")
                .Columns().Begin()
                .AddColumn(_intColumn.name).Separator()
                .AddColumn(_floatColumn.name).Separator()
                .AddColumn(_stringColumn.name).End()
                .Values().Begin()
                .Bind(255 * index).Separator()
                .Bind(3.14F * index).Separator()
                .Bind($"Value of {index}").End()
                .Insert().End(); 
        }
    }
}