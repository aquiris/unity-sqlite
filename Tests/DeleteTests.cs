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
            CreateDatabase();
            _database.Open();

            Query query = new Table()
                .Begin(TableMode.create)
                .Name("Test")
                .Columns().Begin()
                .DeclareColumn("Column1", DataType.Integer).Separator()
                .DeclareColumn("Column2", DataType.Text)
                .End()
                .Table().Build();
            
            SQLiteTable.Run(query, _database, result =>
            {
                Assert.IsTrue(result.success);
            });
            
            Query[] queries = new Query[2];
            queries[0] = new Insert()
                .Begin(InsertMode.insert)
                .IntoTable("Test")
                .Columns().Begin()
                .AddColumn("Column1").Separator()
                .AddColumn("Column2").End()
                .Values().Begin()
                .Bind(1).Separator()
                .Bind("EU").End()
                .Insert().Build();


            queries[1] = new Insert()
                .Begin(InsertMode.insert)
                .IntoTable("Test")
                .Columns().Begin()
                .AddColumn("Column1").Separator()
                .AddColumn("Column2").End()
                .Values().Begin()
                .Bind(2).Separator()
                .Bind("Meu amigo").End()
                .Insert().Build();
            
            SQLiteInsert.Run(queries, _database, result =>
            {
                Assert.IsTrue(result.success);
            });
            
            query = new Delete()
                .Begin()
                .From()
                .Table("Test")
                .Where()
                .Column("Column2")
                .Equal()
                .Binding("Meu amigo")
                .Delete()
                .Build();

            SQLiteDelete.Run(query, _database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(1, result.value);
            });
            
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
            });
        }
    }
}