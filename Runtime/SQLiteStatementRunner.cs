using System;
using System.Threading;
using Aquiris.SQLite.Threading;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Aquiris.SQLite
{
    internal interface ISQLiteQuery
    {
        string statement { get; }
    }
    
    internal abstract class SQLiteStatementRunner
    {
        private readonly Action _completedAction = default;

        private QueryResult _result = default;

        protected SQLiteStatementRunner()
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

        protected abstract object ExecuteThreaded(SqliteCommand command); 
        
        protected abstract void Completed(QueryResult _);

        private void ThreadPoolRunner(object state)
        {
            WorkItemInfo info = (WorkItemInfo) state;
            
            SqliteCommand command = info.database.CreateCommand();
            command.CommandText = info.query.statement;
            
            try
            {
                _result.value = ExecuteThreaded(command);
                _result.success = true;
                _result.errorCode = SQLiteErrorCode.Ok;
                _result.errorMessage = null;
            }
            catch (SqliteException ex)
            {
#if UNITY_EDITOR
                Debug.LogError(ex);                
#endif
                _result.success = false;
                _result.errorCode = ex.ErrorCode;
                _result.errorMessage = ex.Message;
                _result.value = null;
            }

            ThreadSafety.RunOnMainThread(_completedAction);
        }

        private void Completed() => Completed(_result);

        private struct WorkItemInfo
        {
            public SQLiteDatabase database;
            public ISQLiteQuery query;
        }
    }
}