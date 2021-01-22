using System;
using System.Collections.Generic;
using Aquiris.SQLite.Fetching;
using Aquiris.SQLite.Queries;
using Aquiris.SQLite.Runtime.Insertion;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Tests.Shared;
using NUnit.Framework;

namespace Aquiris.SQLite.Tests
{
    public class JoinsTests : BaseTestClass
    {
        [Test]
        public void TestInnerJoin()
        {
            CreateWaiter();
            
            CreateDatabase();
            _database.Open();

            Query query = new Table(EditTableType.create)
                .Name("doctors")
                .Columns()
                .Begin()
                .DeclareColumn("id", DataType.Integer, true)
                .DeclareColumn("name", DataType.Text, true)
                .DeclareColumn("degree", DataType.Text, false)
                .End()
                .Table()
                .Build();
            SQLiteTable.Run(query, _database, result =>
            {
                Assert.IsTrue(result.success);
                _waiter.Set();
            });
            
            WaitOne();

            query = new Table(EditTableType.create)
                .Name("visits")
                .Columns()
                .Begin()
                .DeclareColumn("doctor_id", DataType.Integer, true)
                .DeclareColumn("name", DataType.Text, true)
                .DeclareColumn("visit_date", DataType.Text, false)
                .End()
                .Table()
                .Build();
            SQLiteTable.Run(query, _database, result =>
            {
                Assert.IsTrue(result.success);
                _waiter.Set();
            });
            
            WaitOne();

            const int numberOfInsertions = 9;
            Query[] queries = new Query[numberOfInsertions];
            queries[0] = InsertNewDoctor(210, "Dr. John Linga", "M D");
            queries[1] = InsertNewDoctor(211, "Dr. Peter Hall", "MBBS");
            queries[2] = InsertNewDoctor(212, "Dr. Ke Gee", "M D");
            queries[3] = InsertNewDoctor(213, "Dr. Pat Fay", "M D");
            queries[4] = InsertNewVisit(210, "Julia Nayer", "2013-10-15");
            queries[5] = InsertNewVisit(214, "TJ Olson", "2013-10-14");
            queries[6] = InsertNewVisit(215, "John Seo", "2013-10-15");
            queries[7] = InsertNewVisit(212, "James Marlow", "20113-10-16");
            queries[8] = InsertNewVisit(212, "Jason Mallin", "2013-10-12");
            
            SQLiteInsert.Run(queries, _database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(numberOfInsertions, result.value);
                _waiter.Set();
            });
            
            WaitOne();

            query = new Select()
                .Begin()
                .Columns()
                .AddColumn("doctors.id").Separator()
                .AddColumn("doctors.name").Separator()
                .AddColumn("visits.name").As().Alias("visitor_name")
                .Select()
                .From()
                .Name("doctors")
                .Join()
                .Inner()
                .Name("visits")
                .On()
                .Column("doctors.id")
                .Equal()
                .Column("visits.doctor_id")
                .Where()
                .Column("doctors.degree")
                .Equal()
                .Binding("degreeValue", "M D")
                .Select()
                .Build();
            SQLiteFetch.Run(null, query, _database, result =>
            {
                Assert.IsTrue(result.success);
                List<Dictionary<string, object>> results = (List<Dictionary<string, object>>) result.value;
                Assert.IsNotNull(results);
                Assert.AreEqual(3, results.Count);

                Assert.IsTrue(results[0].ContainsKey("id"));
                Assert.IsTrue(results[0].ContainsKey("name"));
                Assert.IsTrue(results[0].ContainsKey("visitor_name"));

                _waiter.Set();
                
                Console.WriteLine(results);
            });
            
            WaitOne();
        }

        private static Query InsertNewDoctor(int id, string name, string degree)
        {
            return new Insert(InsertType.insert)
                .IntoTable("doctors")
                .Columns()
                .Begin()
                .AddColumn("id").Separator()
                .AddColumn("name").Separator()
                .AddColumn("degree")
                .End()
                .Insert()
                .Values()
                .Begin()
                .Add("id", id).Separator()
                .Add("name", name).Separator()
                .Add("degree", degree)
                .End()
                .Insert()
                .Build();
        }

        private static Query InsertNewVisit(int id, string name, string date)
        {
            return new Insert(InsertType.insert)
                .IntoTable("visits")
                .Columns()
                .Begin()
                .AddColumn("doctor_id").Separator()
                .AddColumn("name").Separator()
                .AddColumn("visit_date")
                .End()
                .Insert()
                .Values()
                .Begin()
                .Add("doctor_id", id).Separator()
                .Add("name", name).Separator()
                .Add("visit_date", date)
                .End()
                .Insert()
                .Build();
        }
    }
}