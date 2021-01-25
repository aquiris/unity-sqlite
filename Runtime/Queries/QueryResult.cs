using Mono.Data.Sqlite;

namespace Aquiris.SQLite.Queries
{
    public struct QueryResult
    {
        public bool success { get; internal set; }
        public SQLiteErrorCode errorCode { get; internal set; }
        public string errorMessage { get; internal set; }
        
        public object value { get; internal set; }
    }
}
