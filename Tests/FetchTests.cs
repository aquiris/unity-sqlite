using System.Collections.Generic;
using Aquiris.SQLite.Fetching;
using Aquiris.SQLite.Queries;
using Aquiris.SQLite.Runtime.Insertion;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Tables;
using Aquiris.SQLite.Tests.Shared;
using NUnit.Framework;

namespace Aquiris.SQLite.Tests
{
    public class FetchTests : BaseTestClass
    {
        [Test]
        public void TestSelectData()
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
            
            // now begins the test

            Query query = new Select()
                .Begin()
                .All()
                .From()
                .Table("TestTable")
                .Build();

            SQLiteFetch.Run(null, query, _database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.IsNotNull(result.value);

                List<Dictionary<string, object>> results = (List<Dictionary<string, object>>) result.value;
                Assert.AreEqual(itemCount, results.Count);
                Assert.IsTrue(results[0].ContainsKey("Column1"));
                Assert.IsTrue(results[0].ContainsKey("Column2"));
                Assert.IsTrue(results[0].ContainsKey("Column3"));
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

        private static Insert CreateInsert(Insert insert, int index)
        {
            return insert.Begin(InsertMode.insert)
                .IntoTable("TestTable")
                .Columns().Begin()
                .AddColumn("Column1").Separator()
                .AddColumn("Column2").Separator()
                .AddColumn("Column3").End()
                .Values().Begin()
                .Bind(255 * index).Separator()
                .Bind(3.14F * index).Separator()
                .Bind($"Value of {index}").End()
                .Insert().End(); 
        }
    }
}