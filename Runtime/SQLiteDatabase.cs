using System;
using System.Data;
using System.IO;
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

    public enum CreateResult
    {
        Create,
        AlreadyExists,
        Failure
    }
    
    public class SQLiteDatabase
    {
        private readonly SqliteConnection _connection = default;
        
        public SQLiteDatabase(string filePath)
        {
#if UNITY
            ThreadSafety.Initialize();
#endif
            
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = $"{filePath}",
                Version = 3,
            };
            string connectionString = connectionStringBuilder.ToString();
            _connection = new SqliteConnection(connectionString);
            _connection.StateChange += (sender, args) => Console.WriteLine(args.CurrentState);
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

        internal void PrepareCommand(SqliteCommand command)
        {
            command.Connection = _connection;
        }

        public static CreateResult Create(string databaseFilePath)
        {
            if (File.Exists(databaseFilePath)) return CreateResult.AlreadyExists;
            string parentDirectory = Directory.GetParent(databaseFilePath).ToString();
            if (!Directory.Exists(parentDirectory)) Directory.CreateDirectory(parentDirectory);
            
            try
            {
                SqliteConnection.CreateFile(databaseFilePath);
                return CreateResult.Create;
            }
            catch (SqliteException ex)
            {
#if UNITY_EDITOR
                Debug.LogError(ex);                
#endif
                return CreateResult.Failure;
            }
        }

        public static CreateResult Create(string filePath, out SQLiteDatabase database)
        {
            CreateResult result = Create(filePath);
            if (result == CreateResult.Failure)
            {
                database = null;
                return result;
            }

            database = new SQLiteDatabase(filePath);
            return result;
        }
    }
}
