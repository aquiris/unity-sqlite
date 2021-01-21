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
            _components.Add(new StringComponent(Constants.QueryComponents.PARENTHESIS_OPEN));
        }

        [UsedImplicitly]
        public Columns AddColumn(string name, bool addComma = true)
        {
            _components.Add(new ColumnDefinitionComponent(name));
            if (addComma) _components.Add(new StringComponent(Constants.QueryComponents.COMMA));
            return this;
        }

        [UsedImplicitly]
        public Columns DeclareColumn(string name, DataType type)
        {
            _components.Add(new ColumnDefinitionComponent(name, type));
            return this;
        }
        
        [UsedImplicitly]
        public Table BackToTable()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.PARENTHESIS_CLOSE));
            return new Table(_components);
        }

        [UsedImplicitly]
        public Insert BackToInsert()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.PARENTHESIS_CLOSE));
            return new Insert(_components);
        }
    }
}