using System;

namespace Aquiris.SQLite.Runtime.Tables
{
    internal class SQLiteTableStatementRunner : SQLiteStatementRunner
    {
        private Action _callbackAction = default;
        
        public void Run(ISQLiteQuery query, SQLiteDatabase database, Action callbackAction)
        {
            _callbackAction = callbackAction;
            Run(query, database);
        }
        
        protected override void Completed()
        {
            _callbackAction.Invoke();
        }
    }
}