using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct Limit
    {
        private QueryComponents _components;

        internal Limit(QueryComponents components)
        {
            _components = components;
            _components.Add(new StringComponent(Constants.QueryComponents.LIMIT));
        }

        [UsedImplicitly]
        public Limit Expression(string expression, bool addComma)
        {
            _components.Add(new StringComponent(expression));
            if (addComma) _components.Add(new StringComponent(Constants.QueryComponents.COMMA));
            return this;
        }

        [UsedImplicitly]
        public Limit Offset(string expression)
        {
            _components.Add(new StringComponent(Constants.QueryComponents.OFFSET));
            _components.Add(new StringComponent(expression));
            return this;
        }

        [UsedImplicitly]
        public Select Select()
        {
            return new Select(_components);
        }
    }
}