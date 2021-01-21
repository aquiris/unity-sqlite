using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;
using static Aquiris.SQLite.Shared.Constants;

namespace Aquiris.SQLite.Queries
{
    public struct Table
    {
        private QueryComponents _components;

        internal Table(Table other)
        {
            _components = other._components;
        }

        internal Table(QueryComponents components)
        {
            _components = components;
        }
        
        [UsedImplicitly]
        public static Table Create()
        {
            Table table = new Table();
            table._components.Add(new StringComponent(Constants.QueryComponents.CREATE));
            return table;
        }

        [UsedImplicitly]
        public Table Temporary()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.TEMPORARY));
            return this;
        }

        [UsedImplicitly]
        public Table IfNotExists()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.IF_NOT_EXISTS));
            return this;
        }

        [UsedImplicitly]
        public Table TableName(string name)
        {
            _components.Add(new StringComponent(name));
            return this;
        }

        [UsedImplicitly]
        public As As()
        {
            return new As(_components);
        }

        [UsedImplicitly]
        public Columns DefineColumns()
        {
            return new Columns(_components);
        }

        [UsedImplicitly]
        public Table WithoutRowId()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.WITHOUT_ROWID));
            return this;
        }

        [UsedImplicitly]
        public Query Build() => _components.Build();
    }
}