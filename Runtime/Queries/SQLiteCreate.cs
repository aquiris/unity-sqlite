using System.Collections.Generic;
using Aquiris.SQLite.Tables;

namespace Aquiris.SQLite.Queries
{
    public readonly struct SQLiteCreate : IQueryComponent
    {
        public KeyValuePair<string, object>[] bindings { get; }
        public int bindingCount { get; }
        
        public SQLiteTable Table(string name, SQLiteColumn[] columns)
        {
            SQLiteQueryComponents comps = new SQLiteQueryComponents();
            comps.Add(this);
            return new SQLiteTable(comps, name);
        }

        public SQLiteTable TemporaryTable(string name, SQLiteColumn[] columns)
        {
            SQLiteQueryComponents comps = new SQLiteQueryComponents();
            comps.Add(this);
            return new SQLiteTable(comps, name, true);
        }

        public SQLiteQuery Finish()
        {
            SQLiteQueryComponents comps = new SQLiteQueryComponents();
            comps.Add(this);
            return comps.Build();
        }

        public string Build()
        {
            return "CREATE";
        }
    }
}