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
        
        internal string bindingName { get; private set; }

        public SQLiteColumn(string name, DataType dataType)
        {
            this.name = name;
            this.dataType = dataType;
            bindingName = $"@{name}";
        }

        public void Rename(string newName, SQLiteTable table, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            Query query = new Query($"ALTER TABLE {table.name} RENAME COLUMN {name} TO {newName}");
            _runner.Run(query, database, onCompleteAction);
            name = newName;
            bindingName = $"@{name}";
        }

        internal string GetTableDeclaration()
        {
            return $"{name} {dataType.Convert()}";
        }

        internal static string GetCreateTableColumnsStatement(SQLiteColumn[] columns, int count)
        {
            string newLine = Constants.newLine;
            string commaNewLine = Constants.commaNewLine;
            
            string statement = "(";
            for (int index = 0; index < count; index++)
            {
                SQLiteColumn column = columns[index];
                statement += column.GetTableDeclaration();
                if (index < count - 1)
                {
                    statement += commaNewLine;
                    continue;
                }
                statement += newLine;
            }
            statement += ")";
            return statement;
        }
    }
}

