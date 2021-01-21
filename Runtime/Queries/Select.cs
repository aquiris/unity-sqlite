using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct Select
    {
        private QueryComponents _components;

        internal Select(QueryComponents components)
        {
            _components = components;
        }

        [UsedImplicitly]
        public Select Distinct()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.SELECT));
            _components.Add(new StringComponent(Constants.QueryComponents.DISTINCT));
            return this;
        }

        [UsedImplicitly]
        public Select All()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.SELECT));
            _components.Add(new StringComponent(Constants.QueryComponents.ALL));
            return this;
        }

        [UsedImplicitly]
        public Select From()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.FROM));
            return this;
        }

        [UsedImplicitly]
        public Select Name(string tableName)
        {
            _components.Add(new StringComponent(tableName));
            return this;
        }

        [UsedImplicitly]
        public Where Where()
        {
            return new Where(_components);
        }

        [UsedImplicitly]
        public Columns Columns()
        {
            return new Columns(_components);
        }

        [UsedImplicitly]
        public Query Build() => _components.Build();
    }
}