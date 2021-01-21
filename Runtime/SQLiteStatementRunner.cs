using System;
using System.Collections.Generic;
using System.Threading;
using Aquiris.SQLite.Queries;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Aquiris.SQLite
{
    internal abstract class SQLiteStatementRunner
    {
        private static readonly object _lock = new object();
        
        private readonly Action _completedAction = default;

        private QueryResult _result = default;

        protected SQLiteStatementRunner()
        {
            _completedAction = Completed;
        }
        
        protected void Run(Query query, SQLiteDatabase database)
        {
            WorkItemInfo state = new WorkItemInfo
            {
                database = database,
                query = query,
                queriesCount = 1,
            };
            ThreadPool.QueueUserWorkItem(ThreadPoolRunner, state);
        }

        protected void Run(Query[] queries, int count, SQLiteDatabase database)
        {
            WorkItemInfo state = new WorkItemInfo
            {
                database = database,
                queries = queries,
                queriesCount = count,
            };
            ThreadPool.QueueUserWorkItem(ThreadPoolRunner, state);
        }

        protected abstract object ExecuteThreaded(SqliteCommand command); 
        
        protected abstract void Completed(QueryResult _);

        private void ThreadPoolRunner(object state)
        {
            lock (_lock)
            {
                WorkItemInfo info = (WorkItemInfo) state;

                using (SqliteTransaction transaction = info.database.BeginTransaction())
                using (SqliteCommand command = info.database.CreateCommand())
                {
                    if (info.queriesCount == 1)
                    {
                        ExecuteOne(command, transaction, info.query);
                    }
                    else
                    {
                        for (int index = 0; index < info.queriesCount; index++)
                        {
                            ExecuteOne(command, transaction, info.queries[index]);
                        }
                    }
                    transaction.Commit();
                }
            }

#if UNITY_INCLUDE_TESTS
            _completedAction.Invoke();
#else
            ThreadSafety.RunOnMainThread(_completedAction);
#endif
        }

        private void Completed() => Completed(_result);

        private void ExecuteOne(SqliteCommand command, SqliteTransaction transaction, Query query)
        {
            command.Transaction = transaction;
            command.CommandText = query.statement;
            PrepareParameters(command, query);
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
                Debug.LogWarning($"Query: {query.statement}{Constants.newLine}{ex}");
#endif
                _result.success = false;
                _result.errorCode = ex.ErrorCode;
                _result.errorMessage = ex.Message;
                _result.value = null;
            }
        }

        private static void PrepareParameters(SqliteCommand command, Query query)
        {
            for (int index = 0; index < query.bindingsCount; index++)
            {
                KeyValuePair<string, object> binding = query.bindings[index];
                command.Parameters.AddWithValue(binding.Key, binding.Value);
            }
            command.Prepare();
        }

        private struct WorkItemInfo
        {
            public SQLiteDatabase database;
            public Query query;
            public Query[] queries;
            public int queriesCount;
        }
    }
}
    
