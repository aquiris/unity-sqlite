using System;
using Aquiris.SQLite.Queries;
using Aquiris.SQLite.Runtime.Tables;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Tables;
using JetBrains.Annotations;

namespace Aquiris.SQLite
{
    public struct SQLiteTable
    {
        private static readonly SQLiteTableStatementRunner _runner = new SQLiteTableStatementRunner();

        private SQLiteColumn[] _columns;
        
        [UsedImplicitly] public string name { get; private set; }
        [UsedImplicitly] public SQLiteColumn[] columns => _columns;
        
        public SQLiteTable(string name, SQLiteColumn[] columns)
        {
            this.name = name;
            _columns = columns;
        }
        
        [UsedImplicitly]
        public void Create(SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            SQLiteQuery query = new SQLiteQuery($"CREATE TABLE {name} {SQLiteColumn.GetCreateTableColumnsStatement(_columns)};");
            _runner.Run(query, database, onCompleteAction);
        }

        [UsedImplicitly]
        public void CreateIfNotExists(SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            SQLiteQuery query = new SQLiteQuery($"CREATE TABLE IF NOT EXISTS {name} {SQLiteColumn.GetCreateTableColumnsStatement(_columns)};");
            _runner.Run(query, database, onCompleteAction);
        }

        [UsedImplicitly]
        public void Rename(string newName, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            SQLiteQuery query = new SQLiteQuery($"ALTER TABLE {name} RENAME TO {newName};");
            _runner.Run(query, database, onCompleteAction);
            name = newName;
        }

        [UsedImplicitly]
        public void Drop(SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            SQLiteQuery query = new SQLiteQuery($"DROP TABLE {name};");
            _runner.Run(query, database, onCompleteAction);
        }
        
        public void AddColumn(SQLiteDatabase database, SQLiteColumn column, Action<QueryResult> onCompleteAction)
        {
            SQLiteQuery query = new SQLiteQuery($"ALTER TABLE {name} ADD COLUMN {column.GetTableDeclaration()};");
            _runner.Run(query, database, onCompleteAction);
            
            int previousLength = _columns.Length;
            Array.Resize(ref _columns, previousLength + 1);
            _columns[previousLength] = column;
        }
    }
}

