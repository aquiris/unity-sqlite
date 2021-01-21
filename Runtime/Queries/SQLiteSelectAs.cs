using Aquiris.SQLite.Queries.Components;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public readonly struct SQLiteSelectAs
    {
        private readonly SQLiteQueryComponents _components;
        
        internal SQLiteSelectAs(SQLiteQueryComponents components)
        {
            _components = components;
            _components.Add(new AsComponent());
        }

        [UsedImplicitly]
        public SQLiteSelect Select()
        {
            return new SQLiteSelect(_components);
        }
    }
}