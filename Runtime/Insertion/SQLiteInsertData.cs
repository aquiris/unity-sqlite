using System;
using System.Collections.Generic;
using Aquiris.SQLite.Tables;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Runtime.Insertion
{
    public struct SQLiteInsertData
    {
        private readonly SQLiteColumn[] _columns;
        private readonly KeyValuePair<SQLiteColumn, object>[] _dataBuffer;
        
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
}