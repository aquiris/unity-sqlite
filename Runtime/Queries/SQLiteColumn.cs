using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;

namespace Aquiris.SQLite.Queries
{
    public struct SQLiteColumn
    {
        private SQLiteQueryComponents _components;

        internal SQLiteColumn(SQLiteColumn other)
        {
            _components = other._components;
        }
        
        internal SQLiteColumn(SQLiteQueryComponents components)
        {
            _components = components;
            _components.Add(new ParenthesisComponent(true));
        }

        public SQLiteColumn DefineColumn(string name, SQLiteDataType type)
        {
            _components.Add(new ColumnDefinitionComponent(name, type));
            return this;
        }

        public SQLiteColumn ColumnSeparator()
        {
            _components.Add(new CommaComponent());
            return this;
        }

        public SQLiteTable BackToTable()
        {
            return new SQLiteTable(_components);
        }
    }
}