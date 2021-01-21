using System;
using System.Collections.Generic;

namespace Aquiris.SQLite.Queries
{
    public struct SQLiteAs : IQueryComponent
    {
        private SQLiteSelect _select;

        public KeyValuePair<string, object>[] bindings => _select.bindings;
        public int bindingCount => _select.bindingCount;
        
        internal SQLiteAs(SQLiteSelect select) : this()
        {
            _select = select;
        }

        public string Build()
        {
            return $"AS {_select.Build()}";
        }

        SQLiteQuery IQueryComponent.Finish()
        {
            throw new NotImplementedException();
        }
    }
}