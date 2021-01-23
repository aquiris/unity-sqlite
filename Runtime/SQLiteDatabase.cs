using System;
using System.Data;
using System.IO;
using Aquiris.SQLite.Shared;
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
        Failure,
    }

    public enum CloseResult
    {
        Close,
        AlreadyClose,
        Failure,
    }

    public enum CreateResult
    {
        Create,
        AlreadyExists,
        Failure,
    }
    
    public class SQLiteDatabase
    {
        private readonly SqliteConnection _connection = default;

        public SQLiteDatabase(string filePath)
        {
            ThreadSafety.Initialize();

            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = $"{filePath}",
                Version = 3,
            };
            string connectionString = connectionStringBuilder.ToString();
            _connection = new SqliteConnection(connectionString);
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
#if UNITY_EDITOR
            catch (Exception ex)
            {
                SQLiteLogger.LogError(
                    new SQLiteLogger.LogPart("Error trying to open database with connection string:", Color.white),
                    SQLiteLogger.LogPart.newLine,
                    new SQLiteLogger.LogPart(_connection.ConnectionString, Color.blue),
                    SQLiteLogger.LogPart.newLine,
                    new SQLiteLogger.LogPart(ex, Color.red));
#else
            catch
            {
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
#if UNITY_EDITOR
            catch (Exception ex)
            {
                SQLiteLogger.LogError(
                    new SQLiteLogger.LogPart("Error trying to close database:", Color.white),
                    SQLiteLogger.LogPart.newLine,
                    new SQLiteLogger.LogPart(ex, Color.red));
#else
            catch 
            {
#endif
                return CloseResult.Failure;
            }
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
#if UNITY_EDITOR
            catch (SqliteException ex)
            {
                SQLiteLogger.LogError(
                    new SQLiteLogger.LogPart("Error trying to create database file at path:", Color.white),
                    SQLiteLogger.LogPart.newLine,
                    new SQLiteLogger.LogPart(databaseFilePath, Color.blue),
                    SQLiteLogger.LogPart.newLine,
                    new SQLiteLogger.LogPart(ex, Color.red));                
#else
            catch
            {
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

        internal SqliteCommand CreateCommand() => _connection.CreateCommand();
        internal SqliteTransaction BeginTransaction() => _connection.BeginTransaction();
    }
}
