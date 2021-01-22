using System;
using Aquiris.SQLite.Queries;
using Aquiris.SQLite.Runtime.Tables;
using Aquiris.SQLite.Shared;

namespace Aquiris.SQLite.Tables
{
    public struct SQLiteColumn
    {
        private static readonly SQLiteColumnStatementRunner _runner = new SQLiteColumnStatementRunner();
        
        public string name { get; private set; }
        public DataType dataType { get; }

        public SQLiteColumn(string name, DataType dataType)
        {
            this.name = name;
            this.dataType = dataType;
        }

        public void Rename(string newName, SQLiteTable table, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            Query query = new Table(TableMode.alter)
                .Name(table.name)
                .Columns()
                .Rename(name, newName)
                .Table()
                .Build();
            _runner.Run(query, database, onCompleteAction);
            name = newName;
        }
    }
}

