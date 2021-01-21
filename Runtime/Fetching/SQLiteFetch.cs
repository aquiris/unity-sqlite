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

        public SQLiteFetch(ISQLiteFetchParser parser = null)
        {
            _runner.parser = parser;
        }
        
        [UsedImplicitly]
        public void Fetch(Query query, SQLiteDatabase database, Action<QueryResult> onCompleteAction)
        {
            _runner.Run(query, database, onCompleteAction);
        }
    }
}