using Mono.Data.Sqlite;

namespace Aquiris.SQLite
{
    public class SQLiteDatabase
    {
        private readonly SqliteConnection _connection = default;

        public SQLiteDatabase(string databasePath, string databasePassword = null)
        {
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                Uri = databasePath,
                Password = databasePassword
            };
            _connection = new SqliteConnection(connectionStringBuilder.ToString());
        }
        
        
    }
}