using System.Collections.Generic;

namespace Aquiris.SQLite.Queries
{
    public interface IQueryComponent
    {
        KeyValuePair<string, object>[] bindings { get; }
        int bindingCount { get; }
        
        string Build();
        SQLiteQuery Finish();
    }
}