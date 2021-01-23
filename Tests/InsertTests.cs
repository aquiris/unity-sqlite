using Aquiris.SQLite.Queries;
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
            Values values = new Insert()
                .Begin(InsertMode.Insert)
                .IntoTable("TestTable")
                .Columns().Begin()
                .AddColumn(_intColumn.name).Separator()
                .AddColumn(_floatColumn.name).Separator()
                .AddColumn(_stringColumn.name).End()
                .Values();
            for (int index = 0; index < itemCount; index++)
            {
                values = AddValue(values, index == 0,
                    255 * index,
                    3.14F * index,
                    $"Value of {index}");
            }
            Query query = values.Insert().End().Build();
            SQLiteInsert.Run(query, _database, result =>
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

            query = CreateInsert(new Insert(), 10, InsertMode.Insert, ConflictMode.Abort)
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
        
        private static Insert CreateInsert(Insert insert, int index, InsertMode mode = InsertMode.Insert, ConflictMode? conflictMode = null)
        {
            insert = insert.Begin(mode)
                .IntoTable("TestTable")
                .Columns().Begin()
                .AddColumn(_intColumn.name).Separator()
                .AddColumn(_floatColumn.name).Separator()
                .AddColumn(_stringColumn.name).End()
                .Insert();
            return AddValue(insert.Values(), 
                    true, 
                    255 * index, 
                    3.14F * index, 
                    $"Value of {index}")
                .Insert().End(); 
        }

        private static Values AddValue(Values values, bool first, int column1, float column2, string column3)
        {
            return values.Begin(first)
                .Bind(column1).Separator()
                .Bind(column2).Separator()
                .Bind(column3).End();
        }
    }
}