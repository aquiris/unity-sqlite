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
            Values values = new Insert()
                .Begin(InsertMode.Insert)
                .IntoTable("TestTable")
                .Columns().Begin()
                .AddColumn("Column1").Separator()
                .AddColumn("Column2").Separator()
                .AddColumn("Column3").End()
                .Values();
            for (int index = 0; index < itemCount; index++)
            {
                values = AddValue(values, index == 0,
                    255 * index,
                    3.14F * index,
                    $"Value of {index}");
            }
            SQLiteInsert.Run(values.Insert().Build(), _database, result =>
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

        private static Values AddValue(Values values, bool first, int column1, float column2, string column3)
        {
            return values.Begin(first)
                .Bind(column1).Separator()
                .Bind(column2).Separator()
                .Bind(column3).End();
        }
    }
}