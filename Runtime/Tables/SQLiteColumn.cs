using System;
using Aquiris.SQLite.Runtime.Tables;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Tables
{
    public struct SQLiteColumn
    {
        private static readonly SQLiteColumnStatementRunner _runner = new SQLiteColumnStatementRunner();
        
        public string name { get; }
        public SQLiteDataType dataType { get; }

        public SQLiteColumn(string name, SQLiteDataType dataType)
        {
            this.name = name;
            this.dataType = dataType;
        }

        [UsedImplicitly]
        public void AddColumn(SQLiteTable table, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            string statement = $"ALTER TABLE {table.name} ADD COLUMN {ToString()};";
            _runner.Run(new ColumnQuery(statement), database, onCompleteAction);
        }

        public override string ToString()
        {
            return $"{name} {GetTypeString()}";
        }

        private string GetTypeString()
        {
            switch (dataType)
            {
                case SQLiteDataType.Blob: return "BLOB";
                case SQLiteDataType.Integer: return "INTEGER";
                case SQLiteDataType.Numeric: return "NUMERIC";
                case SQLiteDataType.Real: return "REAL";
                case SQLiteDataType.Text: return "TEXT";
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
        }

        private readonly struct ColumnQuery : ISQLiteQuery
        {
            public string statement { get; }

            public ColumnQuery(string statement) => this.statement = statement;
        }
    }
}