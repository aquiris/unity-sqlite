using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct On
    {
        private QueryComponents _components;

        internal On(QueryComponents components)
        {
            _components = components;
            _components.Add(new StringComponent(Constants.QueryComponents.ON));
        }

        [UsedImplicitly]
        public On Column(string name)
        {
            _components.Add(new StringComponent(name));
            return this;
        }

        [UsedImplicitly]
        public On Equal()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.EQUAL));
            return this;
        }

        [UsedImplicitly]
        public Where Where()
        {
            return new Where(_components);
        }

        [UsedImplicitly]
        public Select Select()
        {
            return new Select(_components);
        }

        [UsedImplicitly]
        public Joins Join()
        {
            return new Joins(_components);
        }
    }
}