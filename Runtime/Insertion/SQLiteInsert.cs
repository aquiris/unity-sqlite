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
        public void Insert(InsertType type, SQLiteInsertData data, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            Columns columns = new Queries.Insert()
                .IntoTable(_table.name)
                .Columns();
        }

        public void Insert(InsertType type, SQLiteInsertData[] collection, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            
        }
    }
}