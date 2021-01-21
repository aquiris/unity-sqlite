using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;
using static Aquiris.SQLite.Shared.Constants;

namespace Aquiris.SQLite.Queries
{
    public struct Columns
    {
        private QueryComponents _components;

        internal Columns(Columns other)
        {
            _components = other._components;
        }
        
        internal Columns(QueryComponents components)
        {
            _components = components;
        }

        [UsedImplicitly]
        public Columns Begin()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.PARENTHESIS_OPEN));
            return this;
        }

        [UsedImplicitly]
        public Columns AddColumn(string name, bool addComma = true)
        {
            _components.Add(new ColumnDefinitionComponent(name));
            if (addComma) _components.Add(new StringComponent(Constants.QueryComponents.COMMA));
            return this;
        }

        [UsedImplicitly]
        public Columns DeclareColumn(string name, DataType type, bool addComma)
        {
            _components.Add(new ColumnDefinitionComponent(name, type));
            if (addComma) _components.Add(new StringComponent(Constants.QueryComponents.COMMA));
            return this;
        }

        [UsedImplicitly]
        public Columns Rename(string name, string newName)
        {
            _components.Add(new StringComponent(Constants.QueryComponents.RENAME_COLUMN));
            _components.Add(new StringComponent(name));
            _components.Add(new StringComponent(Constants.QueryComponents.TO));
            _components.Add(new StringComponent(newName));
            return this;
        }

        [UsedImplicitly]
        public Columns End()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.PARENTHESIS_CLOSE));
            return this;
        }
        
        [UsedImplicitly]
        public Table Table()
        {
            return new Table(_components);
        }

        [UsedImplicitly]
        public Insert Insert()
        {
            return new Insert(_components);
        }
    }
}