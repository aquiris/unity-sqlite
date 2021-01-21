using System.Collections.Generic;
using Aquiris.SQLite.Queries.Components;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct SQLiteSelect
    {
        private SQLiteQueryComponents _components;

        internal SQLiteSelect(SQLiteQueryComponents components)
        {
            _components = components;
            _components.Add(new SelectComponent());
        }

        [UsedImplicitly]
        public SQLiteColumn DefineColumns()
        {
            return new SQLiteColumn(_components);
        }

        [UsedImplicitly]
        public SQLiteQuery Build() => _components.Build();
    }
}