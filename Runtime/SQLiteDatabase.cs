using Mono.Data.Sqlite;

namespace Aquiris.SQLite
{
    public class SQLiteDatabase
    {
        private readonly SqliteConnection _connection = default;

        public SQLiteDatabase(string databasePath)
        {
            
            _connection = new SqliteConnection(databasePath);
        }
    }
}