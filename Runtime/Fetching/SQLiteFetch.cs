using System;
using Aquiris.SQLite.Queries;
using JetBrains.Annotations;
using Mono.Data.Sqlite;

namespace Aquiris.SQLite.Fetching
{
    public interface ISQLiteFetchParser
    {
        object Parse(SqliteDataReader reader);
    }
    
    public readonly struct SQLiteFetch
    {
        private static readonly SQLiteFetchStatementRunner _runner = new SQLiteFetchStatementRunner();

        [UsedImplicitly]
        public static void Run(Query query, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            Run(query, database, null, onCompleteAction);
        }
        
        [UsedImplicitly]
        public static void Run(Query query, SQLiteDatabase database, ISQLiteFetchParser parser, Action<QueryResult> onCompleteAction)
        {
            _runner.parser = parser;
            _runner.Run(query, database, onCompleteAction);
        }

        public static QueryResult SyncRun(Query query, SQLiteDatabase database)
        {
            return _runner.RunSync(query, database);
        }
    }
}
