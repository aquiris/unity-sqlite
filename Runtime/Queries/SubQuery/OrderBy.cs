using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct OrderBy
    {
        private QueryComponents _components;

        internal OrderBy(QueryComponents components)
        {
            _components = components;
            _components.Add(new StringComponent(Constants.QueryComponents.ORDER_BY));
        }

        [UsedImplicitly]
        public OrderBy Expression(string expression)
        {
            _components.Add(new StringComponent(expression));
            return this;
        }

        [UsedImplicitly]
        public OrderBy Collate(string name)
        {
            _components.Add(new StringComponent(Constants.QueryComponents.COLLATE));
            _components.Add(new StringComponent(name));
            return this;
        }

        [UsedImplicitly]
        public OrderBy Ascending()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.ASCENDING));
            return this;
        }

        [UsedImplicitly]
        public OrderBy Descending()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.DESCENDING));
            return this;
        }

        [UsedImplicitly]
        public OrderBy NullsFirst()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.NULLS_FIRST));
            return this;
        }

        [UsedImplicitly]
        public OrderBy NullsLast()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.NULLS_LAST));
            return this;
        }

        [UsedImplicitly]
        public Select Select()
        {
            return new Select(_components);
        }
    }
}
