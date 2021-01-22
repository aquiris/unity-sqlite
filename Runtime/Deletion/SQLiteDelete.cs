using System;
using Aquiris.SQLite.Queries;

namespace Aquiris.SQLite.Deletion
{
    public struct SQLiteDelete
    {
        private static readonly SQLiteDeleteStatementRunner _runner = new SQLiteDeleteStatementRunner();

        public static void Run(Query query, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            _runner.Run(query, database, onCompleteAction);
        }
    }
}