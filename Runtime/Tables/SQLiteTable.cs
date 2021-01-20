using System;
using Aquiris.SQLite.Runtime.Tables;
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
            string statement = $"CREATE TABLE {name} {CreateColumnsStatement(_columns)};";
            _runner.Run(new TableQuery(statement), database, onCompleteAction);
        }

        [UsedImplicitly]
        public void CreateIfNotExists(SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            string statement = $"CREATE TABLE IF NOT EXISTS {name} {CreateColumnsStatement(_columns)};";
            _runner.Run(new TableQuery(statement), database, onCompleteAction);
        }

        [UsedImplicitly]
        public void Rename(string newName, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            string statement = $"ALTER TABLE {name} RENAME TO {newName};";
            _runner.Run(new TableQuery(statement), database, onCompleteAction);
        }

        [UsedImplicitly]
        public void Drop(SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            string statement = $"DROP TABLE {name};";
            _runner.Run(new TableQuery(statement), database, onCompleteAction);
        }
        
        public void AddColumn(SQLiteDatabase database, SQLiteColumn column, Action<QueryResult> onCompleteAction)
        {
            string statement = $"ALTER TABLE {name} ADD COLUMN {column};";
            _runner.Run(new TableQuery(statement), database, onCompleteAction);
            
            int previousLength = _columns.Length;
            Array.Resize(ref _columns, previousLength + 1);
            _columns[previousLength] = column;
        }
        
        private static string CreateColumnsStatement(SQLiteColumn[] columns)
        {
            string statement = "(";
            for (int index = 0; index < columns.Length; index++)
            {
                SQLiteColumn column = columns[index];
                statement += $"{column.ToString()}";
                if (index < columns.Length - 1)
                {
                    statement += $", {Environment.NewLine}";
                    continue;
                }

                statement += Environment.NewLine;
            }
            statement += ")";
            return statement;
        }

        private readonly struct TableQuery : ISQLiteQuery
        {
            public string statement { get; }

            public TableQuery(string statement) => this.statement = statement;
        }
    }
}
