using System;
using System.Collections.Generic;
using Aquiris.SQLite.Queries;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Tables;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Runtime.Insertion
{
    public readonly struct SQLiteInsert
    {
        private static readonly Query[] _queriesBuffer = new Query[Constants.maxNumberOfQueries];
        private static readonly SQLiteInsertStatementRunner _runner = new SQLiteInsertStatementRunner();

        private readonly SQLiteTable _table;

        public SQLiteInsert(SQLiteTable table)
        {
            _table = table;
        }

        [UsedImplicitly]
        public void Insert(Query query, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            _runner.Run(query, database, onCompleteAction);
        }

        [UsedImplicitly]
        public void Insert(InsertType type, SQLiteInsertData data, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            Insert insert = new Insert(type).IntoTable(_table.name);
            Query query = DoValues(DoColumns(insert, data), data).Build();
            _runner.Run(query, database, onCompleteAction);
        }

        [UsedImplicitly]
        public void Insert(Query[] queries, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            _runner.Run(queries, queries.Length, database, onCompleteAction);
        }

        public void Insert(InsertType type, SQLiteInsertData[] collection, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            for (int index = 0; index < collection.Length; index++)
            {
                SQLiteInsertData data = collection[index];
                Insert insert = new Insert(type).IntoTable(_table.name);
                Query query = DoValues(DoColumns(insert, data), data).Build();
                _queriesBuffer[index] = query;
            }
            _runner.Run(_queriesBuffer, collection.Length, database, onCompleteAction);
        }

        private static Insert DoColumns(Insert insert, SQLiteInsertData data)
        {
            Columns cols = insert.Columns().Begin();
            for (int index = 0; index < data.dataCount; index++)
            {
                KeyValuePair<SQLiteColumn, object> pair = data.data[index];
                bool addComma = index < data.dataCount - 1;
                cols = cols.AddColumn(pair.Key.name, addComma);
            }
            return cols.End().Insert();
        }

        private static Insert DoValues(Insert insert, SQLiteInsertData data)
        {
            Values values = insert.Values().Begin();
            for (int index = 0; index < data.dataCount; index++)
            {
                KeyValuePair<SQLiteColumn, object> pair = data.data[index];
                bool addComma = index < data.dataCount - 1;
                values = values.Add(pair.Key.name, pair.Value, addComma);
            }
            return values.End().Insert();
        }
    }
}