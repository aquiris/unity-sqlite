using System;
using System.Threading;
using Aquiris.SQLite.Threading;
using Mono.Data.Sqlite;

namespace Aquiris.SQLite
{
    internal interface ISQLiteQuery
    {
        string statement { get; }
    }
    
    internal abstract class SQLiteStatementRunner
    {
        private readonly Action _completedAction = default;

        public SQLiteStatementRunner()
        {
            _completedAction = Completed;
        }
        
        protected void Run(ISQLiteQuery query, SQLiteDatabase database)
        {
            WorkItemInfo state = new WorkItemInfo
            {
                database = database,
                query = query
            };
            ThreadPool.QueueUserWorkItem(ThreadPoolRunner, state);
        }

        protected abstract void Completed();

        private void ThreadPoolRunner(object state)
        {
            WorkItemInfo info = (WorkItemInfo) state;
            
            SqliteCommand command = info.database.CreateCommand();
            command.CommandText = info.query.statement;
            command.ExecuteNonQuery();
            
            ThreadSafety.RunOnMainThread(_completedAction);
        }

        private struct WorkItemInfo
        {
            public SQLiteDatabase database;
            public ISQLiteQuery query;
        }
    }
}