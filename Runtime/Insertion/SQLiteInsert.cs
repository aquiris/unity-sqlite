using System;
using System.Collections.Generic;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Tables;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Runtime.Insertion
{
    public readonly struct SQLiteInsert
    {
        private static readonly SQLiteQuery[] _queriesBuffer = new SQLiteQuery[Constants.maxNumberOfQueries];
        private static readonly SQLiteInsertStatementRunner _runner = new SQLiteInsertStatementRunner();
        
        private readonly SQLiteTable _table;

        public SQLiteInsert(SQLiteTable table)
        {
            _table = table;
        }

        [UsedImplicitly]
        public void Insert(SQLiteInsertData data, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            SQLiteQuery query = CreateQuery(data);
            _runner.Run(query, database, onCompleteAction);
        }

        public void Insert(SQLiteInsertData[] collection, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            int queriesLength = collection.Length;
            for (int index = 0; index < queriesLength; index++)
            {
                _queriesBuffer[index] = CreateQuery(collection[index]);
            }
            _runner.Run(_queriesBuffer, queriesLength, database, onCompleteAction);
        }
        
        private SQLiteQuery CreateQuery(SQLiteInsertData data)
        {
            SQLiteQuery query = new SQLiteQuery();
            CreateInsertDataStatement(data, ref query, out string columns, out string values);
            query.statement = $"INSERT INTO {_table.name} {columns} VALUES {values};";
            return query;
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