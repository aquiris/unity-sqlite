using System.Collections.Generic;
using Aquiris.SQLite.Deletion;
using Aquiris.SQLite.Fetching;
using Aquiris.SQLite.Queries;
using Aquiris.SQLite.Runtime.Insertion;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Tests.Shared;
using NUnit.Framework;

namespace Aquiris.SQLite.Tests
{
    public class DeleteTests : BaseTestClass
    {
        [Test]
        public void TestDeleteRow()
        {
            CreateWaiter();
            
            CreateDatabase();
            _database.Open();

            Query query = new Table(TableMode.create)
                .Name("Test")
                .Columns().Begin()
                .DeclareColumn("Column1", DataType.Integer).Separator()
                .DeclareColumn("Column2", DataType.Text)
                .End()
                .Table().Build();
            
            SQLiteTable.Run(query, _database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();

            query = new Insert(InsertMode.insert)
                .IntoTable("Test")
                .Columns().Begin()
                .AddColumn("Column1").Separator()
                .AddColumn("Column2").End()
                .Values().Begin()
                .Add("Column1", 1).Separator()
                .Add("Column2", "EU")
                .End()
                .Insert().Build();

            Query[] queries = new Query[2];
            queries[0] = query;
            
            query = new Query(query.statement);
            query.Bind("@Column10", 2);
            query.Bind("@Column21", "Meu amigo");
            queries[1] = query;
            
            SQLiteInsert.Run(queries, _database, result =>
            {
                Assert.IsTrue(result.success);
                WaiterSet();
            });
            
            WaitOne();

            query = new Delete()
                .Begin()
                .From()
                .Table("Test")
                .Where()
                .Column("Column2")
                .Equal()
                .Binding("Column2", "Meu amigo")
                .Delete()
                .Build();

            SQLiteDelete.Run(query, _database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(1, result.value);
                
                WaiterSet();
            });
            
            WaitOne();

            query = new Select()
                .Begin()
                .All()
                .From()
                .Table("Test")
                .Build();
            
            SQLiteFetch.Run(null, query, _database, result =>
            {
                Assert.IsTrue(result.success);

                List<Dictionary<string, object>> results = (List<Dictionary<string, object>>) result.value;
                
                Assert.IsNotNull(results);
                Assert.AreEqual(1, results.Count);
                
                WaiterSet();
            });
            
            WaitOne();
        }
    }
}