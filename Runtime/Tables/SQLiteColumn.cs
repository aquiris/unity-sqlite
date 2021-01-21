using System;
using Aquiris.SQLite.Runtime.Tables;

namespace Aquiris.SQLite.Tables
{
    public struct SQLiteColumn
    {
        private static readonly SQLiteColumnStatementRunner _runner = new SQLiteColumnStatementRunner();
        
        public string name { get; private set; }
        public SQLiteDataType dataType { get; }
        
        internal string bindingName { get; private set; }

        public SQLiteColumn(string name, SQLiteDataType dataType)
        {
            this.name = name;
            this.dataType = dataType;
            bindingName = $"@{name}";
        }

        public void Rename(string newName, SQLiteTable table, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            SQLiteQuery query = new SQLiteQuery($"ALTER TABLE {table.name} RENAME COLUMN {name} TO {newName}");
            _runner.Run(query, database, onCompleteAction);
            name = newName;
            bindingName = $"@{name}";
        }

        internal string GetTableDeclaration()
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
    }
}

