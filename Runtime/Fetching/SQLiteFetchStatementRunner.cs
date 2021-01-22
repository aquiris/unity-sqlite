using System;
using System.Collections.Generic;
using Aquiris.SQLite.Queries;
using Mono.Data.Sqlite;

namespace Aquiris.SQLite.Fetching
{
    public class SQLiteFetchStatementRunner : SQLiteStatementRunner
    {
        private Action<QueryResult> _callbackAction;
        
        public ISQLiteFetchParser parser { get; set; }
        
        public void Run(Query query, SQLiteDatabase database, Action<QueryResult> callbackAction)
        {
            _callbackAction = callbackAction;
            Run(query, database);
        }

        protected override object ExecuteThreaded(SqliteCommand command)
        {
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows) return null;
                if (parser != null) return parser.Parse(reader);
                List<Dictionary<string, object>> queryResults = new List<Dictionary<string, object>>();
                while (reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    for (int index = 0; index < reader.VisibleFieldCount; index++)
                    {
                        string name = reader.GetName(index);
                        object value = reader.GetValue(index);
                        row[name] = value;
                    }

                    queryResults.Add(row);
                }
                return queryResults;
            }
        }

        protected override void Completed(QueryResult result)
        {
            parser = null;
            _callbackAction.Invoke(result);
        }
    }
}