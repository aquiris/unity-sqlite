using System;
using Aquiris.SQLite.Runtime.Tables;
using Aquiris.SQLite.Tables;
using JetBrains.Annotations;

namespace Aquiris.SQLite
{
    public readonly struct SQLiteTable
    {
        private static readonly SQLiteTableStatementRunner _runner = new SQLiteTableStatementRunner();
        
        [UsedImplicitly] public string name { get; }
        [UsedImplicitly] public SQLiteColumn[] columns { get; }
        
        public SQLiteTable(string name, SQLiteColumn[] columns)
        {
            this.name = name;
            this.columns = columns;
        }
        
        [UsedImplicitly]
        public void Create(SQLiteDatabase database, Action onCompleteAction)
        {
            string statement = $"CREATE TABLE {name} {CreateColumnsStatement()};";
            _runner.Run(new TableQuery(statement), database, onCompleteAction);
        }

        [UsedImplicitly]
        public void CreateIfNotExists(SQLiteDatabase database, Action onCompleteAction)
        {
            string statement = $"CREATE TABLE IF NOT EXISTS {name} {CreateColumnsStatement()};";
            _runner.Run(new TableQuery(statement), database, onCompleteAction);
        }

        private string CreateColumnsStatement()
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
            return statement;
        }

        private readonly struct TableQuery : ISQLiteQuery
        {
            public string statement { get; }

            public TableQuery(string statement)
            {
                this.statement = statement;
            }
        }
    }
}