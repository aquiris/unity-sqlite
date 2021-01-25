using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Aquiris.SQLite.Queries;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Threading;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Aquiris.SQLite
{
    public abstract class SQLiteStatementRunner
    {
        private static readonly object _lock = new object();
        private readonly ObjectPool<WorkItemInfo> _workItemPool = new ObjectPool<WorkItemInfo>(() => new WorkItemInfo());
        private readonly WaitCallback _threadPoolRunner = default;
        private readonly Action _completedAction = default;
        private QueryResult _result = default;

        protected SQLiteStatementRunner()
        {
            _completedAction = Completed;
            _threadPoolRunner = ThreadPoolRunner;
        }

        protected void Run(Query query, SQLiteDatabase database)
        {
            WorkItemInfo state = _workItemPool.Rent();
            state.database = database;
            state.query = query;
            state.queriesCount = 1;
            if (ThreadSafety.isPlaying)
            {
                ThreadPool.QueueUserWorkItem(_threadPoolRunner, state);
                return;
            }

            _threadPoolRunner.Invoke(state);
        }

        protected void Run(Query[] queries, int count, SQLiteDatabase database)
        {
            WorkItemInfo state = _workItemPool.Rent();
            state.database = database;
            state.queries = queries;
            state.queriesCount = count;
            if (ThreadSafety.isPlaying)
            {
                ThreadPool.QueueUserWorkItem(_threadPoolRunner, state);
                return;
            }

            _threadPoolRunner.Invoke(state);
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
                    command.Transaction = transaction;
                    if (info.queriesCount == 1) ExecuteOnce(command, info.query);
                    else ExecuteMany(command, info.queries, info.queriesCount);
                    transaction.Commit();
                }

                _workItemPool.Pay(info);
            }

            ThreadSafety.RunOnMainThread(_completedAction);
        }

        private void Completed() => Completed(_result);

        private void ExecuteOnce(SqliteCommand command, Query query)
        {
            PrepareParameters(command, query);
            try
            {
                SQLiteLogger.Log(new SQLiteLogger.LogPart("Query: ", Color.white),
                    new SQLiteLogger.LogPart(query.statement, Constants.Colors.lightBlue),
                    SQLiteLogger.LogPart.newLine,
                    new SQLiteLogger.LogPart("Executed successfully", Color.white));

                _result.value = ExecuteThreaded(command);
                _result.success = true;
                _result.errorCode = SQLiteErrorCode.Ok;
                _result.errorMessage = null;
            }
            catch (SqliteException ex)
            {
                SQLiteLogger.LogWarning(new SQLiteLogger.LogPart("Query: ", Color.white),
                    new SQLiteLogger.LogPart(query.statement, Constants.Colors.lightBlue),
                    SQLiteLogger.LogPart.newLine,
                    new SQLiteLogger.LogPart("Execution failed with error: ", Color.white),
                    SQLiteLogger.LogPart.newLine,
                    new SQLiteLogger.LogPart(ex, Color.red));

                _result.success = false;
                _result.errorCode = ex.ErrorCode;
                _result.errorMessage = ex.Message;
                _result.value = null;
            }
        }

        private void ExecuteMany(SqliteCommand command, Query[] queries, int count)
        {
            for (int index = 0; index < count; index++)
            {
                ExecuteOnce(command, queries[index]);
            }
        }

        private static void PrepareParameters(SqliteCommand command, Query query)
        {
            command.CommandText = query.statement;
            for (int index = 0; index < query.bindingCount; index++)
            {
                KeyValuePair<string, object> binding = query.bindings[index];
                (DbType type, int size) = GetDataInfo(binding.Value);
                command.Parameters.Add(binding.Key, type, size).Value = binding.Value;
            }

            command.Prepare();
        }

        private static (DbType, int) GetDataInfo(object value)
        {
            if (value == null)
            {
                return (DbType.Object, 0);
            }

            switch (value)
            {
                case int _: return (DbType.Int32, sizeof(int));
                case float _: return (DbType.Double, sizeof(float));
                case string stringValue: return (DbType.String, stringValue.Length);
                case byte[] bytesValue: return (DbType.Binary, bytesValue.Length);
                default:
                    string message = $"Unexpected type: {value.GetType()}\n" +
                                     $"Supported types are:\n" +
                                     $"\tint\n" +
                                     $"\tfloat\n" +
                                     $"\tstring\n" +
                                     $"\tbyte[]\n";
                    throw new ArgumentOutOfRangeException(nameof(value), value, message);
            }
        }

        private class WorkItemInfo
        {
            public SQLiteDatabase database = default;
            public Query query = default;
            public Query[] queries = default;
            public int queriesCount = default;
        }
    }
}
