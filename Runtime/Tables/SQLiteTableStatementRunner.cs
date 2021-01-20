using System;
using Mono.Data.Sqlite;

namespace Aquiris.SQLite.Runtime.Tables
{
    internal class SQLiteTableStatementRunner : SQLiteStatementRunner
    {
        private Action<QueryResult> _callbackAction = default;
        
        public void Run(ISQLiteQuery query, SQLiteDatabase database, Action<QueryResult> callbackAction)
        {
            _callbackAction = callbackAction;
            Run(query, database);
        }

        protected override object ExecuteThreaded(SqliteCommand command)
        {
            command.ExecuteNonQuery();
            return null;
        }

        protected override void Completed(QueryResult result)
        {
            _callbackAction.Invoke(result);
        }
    }
}
