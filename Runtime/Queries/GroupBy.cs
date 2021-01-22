using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct GroupBy
    {
        private QueryComponents _components;

        internal GroupBy(QueryComponents components)
        {
            _components = components;
            _components.Add(new StringComponent(Constants.QueryComponents.GROUP_BY));
        }

        [UsedImplicitly]
        public GroupBy Expression(string expression)
        {
            _components.Add(new StringComponent(expression));
            return this;
        }

        [UsedImplicitly]
        public GroupBy Separator()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.COMMA));
            return this;
        }

        [UsedImplicitly]
        public GroupBy Having()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.HAVING));
            return this;
        }

        [UsedImplicitly]
        public Select Select()
        {
            return new Select(_components);
        } 
    }
}