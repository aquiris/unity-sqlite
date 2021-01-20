using System;
using System.Collections.Generic;
using System.Linq;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Tables;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Runtime.Insertion
{
    public struct SQLiteInsertData
    {
        private SQLiteColumn[] _columns;
        private KeyValuePair<SQLiteColumn, object>[] _dataBuffer;
        
        public int dataCount { get; private set; }
        public IReadOnlyList<KeyValuePair<SQLiteColumn, object>> data => _dataBuffer;

        public SQLiteInsertData(SQLiteTable table)
        {
            _columns = table.columns;
            _dataBuffer = new KeyValuePair<SQLiteColumn, object>[_columns.Length];
            dataCount = 0;
        }
        
        [UsedImplicitly]
        public void Add(string column, object value)
        {
            _dataBuffer[dataCount] = new KeyValuePair<SQLiteColumn, object>(GetColumn(column), value);
            dataCount += 1;
        }
    
        [UsedImplicitly]
        public void Add(SQLiteColumn column, object value) => Add(column.name, value);

        private SQLiteColumn GetColumn(string name)
        {
            for (int index = 0; index < _columns.Length; index++)
            {
                SQLiteColumn column = _columns[index];
                if (column.name.Equals(name)) return column;
            }
            throw new ArgumentOutOfRangeException(nameof(name), name, $"Column not found");
        }
    }
    
    public readonly struct SQLiteInsert
    {
        private static readonly SQLiteInsertStatementRunner _runner = new SQLiteInsertStatementRunner();
        
        private readonly SQLiteTable _table;

        public SQLiteInsert(SQLiteTable table)
        {
            _table = table;
        }

        [UsedImplicitly]
        public void Insert(SQLiteInsertData data, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            SQLiteQuery query = new SQLiteQuery();
            CreateInsertDataStatement(data, ref query, out string columns, out string values);
            query.statement = $"INSERT INTO {_table.name} {columns} VALUES {values}";
            _runner.Run(query, database, onCompleteAction);
        }
        
        private static void CreateInsertDataStatement(SQLiteInsertData insertData, ref SQLiteQuery query, out string columns, out string values)
        {
            string newLine = Constants.newLine;
            string commaNewLine = Constants.commaNewLine;
            columns = "(";
            values = "(";
            IReadOnlyList<KeyValuePair<SQLiteColumn, object>> data = insertData.data;
            for (int index = 0; index < insertData.dataCount; index++)
            {
                KeyValuePair<SQLiteColumn, object> column = data[index];
                string columnName = column.Key.name;
                string bindingName = column.Key.bindingName;
                query.Add(bindingName, column.Value);
                columns += columnName;
                values += bindingName;
                if (index < insertData.dataCount - 1)
                {
                    columns += commaNewLine;
                    values += commaNewLine;
                    continue;
                }
                
                columns += newLine;
                values += newLine;
            }
            columns += ")";
            values += ")";
        }
    }
}