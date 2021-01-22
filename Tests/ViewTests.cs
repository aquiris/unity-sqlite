using Aquiris.SQLite.Queries;
using Aquiris.SQLite.Runtime.Insertion;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Tables;
using Aquiris.SQLite.Tests.Shared;
using NUnit.Framework;

namespace Aquiris.SQLite.Tests
{
    public class ViewTests : BaseTestClass
    {
        [Test]
        public void TestCreateView()
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

            SQLiteInsertData data = new SQLiteInsertData(table);
            data.Add("Column1", 10);
            SQLiteInsert insert = new SQLiteInsert(table);
            insert.Insert(InsertType.insert, data, _database, result =>
            {
                Assert.IsTrue(result.success);
                Assert.AreEqual(1, result.value);
                _waiter.Set();
            });
            
            WaitOne();

            Query query = new Table(EditTableType.create, true)
                .IfNotExists()
                .Name("MyView")
                .As()
                .Select()
                .Begin()
                .Columns()
                .Begin()
                .AddColumn("Column1")
                .End()
                .Table()
                .Build();
            SQLiteTable.Run(query, _database, result =>
            {
                Assert.IsTrue(result.success);
                _waiter.Set();
            });
            
            WaitOne();
        }

        [Test]
        public void TestCreateBigTable()
        {
	        CreateWaiter();
	        CreateDatabase();
	        _database.Open();

	        Table table = new Table(EditTableType.create)
		        .Name("BigTable")
		        .Columns()
		        .Begin()
		        .DeclareColumn("refCharacter", DataType.Text).Separator()
		        .DeclareColumn("skillId", DataType.Text).Separator()
		        .DeclareColumn("dependency", DataType.Text).Separator()
		        .DeclareColumn("skillVersion", DataType.Text).Separator()
		        .DeclareColumn("animationAsset", DataType.Text).Separator()
		        .DeclareColumn("iconAsset", DataType.Text).Separator()
		        .DeclareColumn("name", DataType.Text).Separator()
		        .DeclareColumn("description", DataType.Text).Separator()
		        .DeclareColumn("upgradeDescription", DataType.Text).Separator()
		        .DeclareColumn("type", DataType.Text).Separator()
		        .DeclareColumn("cooldownTurns", DataType.Text).Separator()
		        .DeclareColumn("warmupTurns", DataType.Text).Separator()
		        .DeclareColumn("typeColor", DataType.Text).Separator()
		        .DeclareColumn("moment", DataType.Text).Separator()
		        .DeclareColumn("momentSubject", DataType.Text).Separator()
		        .DeclareColumn("condition", DataType.Text).Separator()
		        .DeclareColumn("conditionSubject", DataType.Text).Separator()
		        .DeclareColumn("conditionParameter", DataType.Text)
		        .End()
		        .Table();
	        Query query = table.Build();
	        SQLiteTable.Run(query, _database, result =>
	        {
				Assert.IsTrue(result.success);
				_waiter.Set();
	        });
	        
	        WaitOne();

	        query = new Table(EditTableType.create, true)
		        .Name("BigView")
		        .As()
		        .Select()
		        .Begin()
		        .Columns()
		        .Begin()
		        .AddColumn("moment").Separator()
		        .AddColumn("type").Separator()
		        .AddColumn("momentSubject")
		        .End()
		        .Table()
		        .Build();
	        SQLiteTable.Run(query, _database, result =>
	        {
		        Assert.IsTrue(result.success);
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