using System;
using Aquiris.SQLite.Queries;
using Mono.Data.Sqlite;

namespace Aquiris.SQLite.Runtime.Insertion
{
    internal class SQLiteInsertStatementRunner : SQLiteStatementRunner
    {
        private Action<QueryResult> _callbackAction = default;
        private int _insertCount = default;
        
        public void Run(Query query, SQLiteDatabase database, Action<QueryResult> callbackAction)
        {
            _insertCount = 0;
            _callbackAction = callbackAction;
            Run(query, database);
        }

        public void Run(Query[] queries, int count, SQLiteDatabase database, Action<QueryResult> callbackAction)
        {
            _insertCount = 0;
            _callbackAction = callbackAction;
            Run(queries, count, database);
        }
        
        protected override object Execute(SqliteCommand command)
        {
            _insertCount += command.ExecuteNonQuery();
            return null;
        }

        protected override void Completed(QueryResult result)
        {
            result.value = _insertCount;
            _callbackAction.Invoke(result);
        }
    }
}
