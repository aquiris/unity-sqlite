using System;
using Aquiris.SQLite.Queries;
using Mono.Data.Sqlite;

namespace Aquiris.SQLite.Deletion
{
    public class SQLiteDeleteStatementRunner : SQLiteStatementRunner
    {
        private Action<QueryResult> _callbackAction;

        public void Run(Query query, SQLiteDatabase database, Action<QueryResult> callbackAction)
        {
            _callbackAction = callbackAction;
            Run(query, database);
        }
        
        protected override object Execute(SqliteCommand command)
        {
            return command.ExecuteNonQuery();
        }

        protected override void Completed(QueryResult result)
        {
            _callbackAction.Invoke(result);
        }
    }
}
