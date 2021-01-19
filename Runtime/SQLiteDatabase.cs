using System;
using System.Data;
using Aquiris.SQLite.Threading;
using JetBrains.Annotations;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Aquiris.SQLite
{
    public enum OpenResult
    {
        Open,
        AlreadyOpen,
        Failure
    }

    public enum CloseResult
    {
        Close,
        AlreadyClose,
        Failure
    }
    
    public class SQLiteDatabase
    {
        private readonly SqliteConnection _connection = default;
        
        public SQLiteDatabase(string databasePath, string databasePassword = null)
        {
            ThreadSafety.Initialize();
            
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                Uri = databasePath,
                Password = databasePassword
            };
            _connection = new SqliteConnection(connectionStringBuilder.ToString());
        }

        [UsedImplicitly]
        public OpenResult Open()
        {
            if (_connection.State == ConnectionState.Connecting || 
                _connection.State == ConnectionState.Open) return OpenResult.AlreadyOpen;
            try
            {
                _connection.Open();
                return OpenResult.Open;
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                Debug.LogError(ex);
#endif
                return OpenResult.Failure;
            }
        }

        [UsedImplicitly]
        public CloseResult Close()
        {
            if (_connection.State == ConnectionState.Broken || 
                _connection.State == ConnectionState.Closed) return CloseResult.AlreadyClose;
            try
            {
                _connection.Close();
                return CloseResult.Close;
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                Debug.LogError(ex);
#endif
                return CloseResult.Failure;
            }
        }

        [UsedImplicitly]
        internal SqliteCommand CreateCommand() => _connection.CreateCommand();
    }
}