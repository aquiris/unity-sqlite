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
            CreateWaiter();
            
            CreateDatabase();
            _database.Open();

            SQLiteTable table = GetTable();
            table.Create(_database, result =>
            {
                Assert.IsTrue(result.success);
                _waiter.Set();
            });
            
            WaitOne();

            SQLiteInsert insert = new SQLiteInsert(table);
            
            const int itemCount = 10000;
            SQLiteInsertData[] collection = new SQLiteInsertData[itemCount];
            for (int index = 0; index < collection.Length; index++)
            {
                SQLiteInsertData data = new SQLiteInsertData(table);
                data.Add("Column1", 255 * index);
                data.Add("Column2", 3.14F * index);
                data.Add("Column3", $"Value of {index}");
                collection[index] = data;
            }
            
            insert.Insert(InsertType.insert, collection, _database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(itemCount, result.value);
                _waiter.Set();
            });
            
            WaitOne();
            
            // now begins the test

            Query query = new Select()
                .Begin()
                .All()
                .From()
                .Name("TestTable")
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
                
                _waiter.Set();
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