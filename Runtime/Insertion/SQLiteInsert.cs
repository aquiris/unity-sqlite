using System;
using Aquiris.SQLite.Queries;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Runtime.Insertion
{
    public readonly struct SQLiteInsert
    {
        private static readonly SQLiteInsertStatementRunner _runner = new SQLiteInsertStatementRunner();

        [UsedImplicitly]
        public static void Run(Query query, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            _runner.Run(query, database, onCompleteAction);
        }
        
        [UsedImplicitly]
        public static void Run(Query[] queries, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            _runner.Run(queries, queries.Length, database, onCompleteAction);
        }
    }
}
