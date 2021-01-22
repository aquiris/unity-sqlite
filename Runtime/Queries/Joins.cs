using System;
using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct Joins
    {
        private QueryComponents _components;

        internal Joins(QueryComponents components)
        {
            _components = components;
        }

        [UsedImplicitly]
        public Joins Left()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.LEFT));
            return this;
        }

        [UsedImplicitly]
        public Joins Outer()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.OUTER));
            return this;
        }

        [UsedImplicitly]
        public Joins Inner()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.INNER));
            return this;
        }

        [UsedImplicitly]
        public Joins Name(string name)
        {
            _components.Add(new StringComponent(Constants.QueryComponents.JOIN));
            _components.Add(new StringComponent(name));
            return this;
        }

        [UsedImplicitly]
        public On On()
        {
            return new On(_components);
        }

        [UsedImplicitly]
        public Where Where()
        {
            return new Where(_components);
        }
    }
}