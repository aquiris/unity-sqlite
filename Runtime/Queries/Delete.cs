using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct Delete
    {
        private QueryComponents _components;

        internal Delete(QueryComponents components)
        {
            _components = components;
        }

        [UsedImplicitly]
        public Delete With()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.WITH));
            return this;
        }

        [UsedImplicitly]
        public Delete Recursive()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.RECURSIVE));
            return this;
        }

        [UsedImplicitly]
        public Delete Begin()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.DELETE));
            return this;
        }

        [UsedImplicitly]
        public Delete From()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.FROM));
            return this;
        }

        [UsedImplicitly]
        public Delete Table(string tableName)
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
        public Query Build()
        {
            return _components.Build();
        }
    }
}