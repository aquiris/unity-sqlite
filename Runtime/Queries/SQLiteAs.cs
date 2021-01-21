using System;
using System.Collections.Generic;

namespace Aquiris.SQLite.Queries
{
    public struct SQLiteAs : IQueryComponent
    {
        private SQLiteSelect _select;
        
        public KeyValuePair<string, object>[] bindings { get; }
        public int bindingCount { get; }
        
        internal SQLiteAs(SQLiteSelect select) : this()
        {
            _select = select;
        }

        public SQLiteQuery Finish()
        {
            throw new NotImplementedException();
        }

        public string Build()
        {
            // TODO(anderson): make this happen
            return "";
        }
    }
}