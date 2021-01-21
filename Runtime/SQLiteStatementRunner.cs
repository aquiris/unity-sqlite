using System;
using System.Collections.Generic;
using System.Threading;
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
        
        protected void Run(SQLiteQuery query, SQLiteDatabase database)
        {
            WorkItemInfo state = new WorkItemInfo
            {
                database = database,
                query = query,
                queriesCount = 1,
            };
            ThreadPool.QueueUserWorkItem(ThreadPoolRunner, state);
        }

        protected void Run(SQLiteQuery[] queries, int count, SQLiteDatabase database)
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

        private void ExecuteOne(SqliteCommand command, SqliteTransaction transaction, SQLiteQuery query)
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
                Debug.LogWarning(ex);
#endif
                _result.success = false;
                _result.errorCode = ex.ErrorCode;
                _result.errorMessage = ex.Message;
                _result.value = null;
            }
        }

        private static void PrepareParameters(SqliteCommand command, SQLiteQuery query)
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
            public SQLiteQuery query;
            public SQLiteQuery[] queries;
            public int queriesCount;
        }
    }
}
    
    internal struct SQLiteQuery
    {
        private static readonly KeyValuePair<string, object>[] _bindingsBuffer = new KeyValuePair<string, object>[Constants.maxNumberOfBindings]; 

        public string statement;
        
        public int bindingsCount { get; private set; }
        public IReadOnlyList<KeyValuePair<string, object>> bindings => _bindingsBuffer;

        public SQLiteQuery(string statement)
        {
            this.statement = statement;
            bindingsCount = 0;
        }

        [UsedImplicitly]
        public void Add(KeyValuePair<string, object> binding)
        {
            _bindingsBuffer[bindingsCount] = binding;
            bindingsCount += 1;
        }

        [UsedImplicitly]
        public void Add(string column, object value) => Add(new KeyValuePair<string, object>(column, value));
    }
    
