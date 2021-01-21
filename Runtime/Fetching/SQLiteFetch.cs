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
        public static void Run(ISQLiteFetchParser parser, Query query, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            _runner.parser = parser;
            _runner.Run(query, database, onCompleteAction);
        }
    }
}