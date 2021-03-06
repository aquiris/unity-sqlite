﻿using Aquiris.SQLite.Queries.Components;
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
        public Select Begin()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.SELECT));
            return this;
        }

        [UsedImplicitly]
        public Select End()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.SEMICOLON));
            return this;
        }

        [UsedImplicitly]
        public Select Distinct()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.DISTINCT));
            return this;
        }

        [UsedImplicitly]
        public Select All()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.SELECT_ALL));
            return this;
        }

        [UsedImplicitly]
        public Select From()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.FROM));
            return this;
        }

        [UsedImplicitly]
        public Select Table(string tableName)
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
        public GroupBy GroupBy()
        {
            return new GroupBy(_components);
        }

        [UsedImplicitly]
        public Window Window()
        {
            return new Window(_components);
        }

        [UsedImplicitly]
        public Values Values()
        {
            return new Values(_components);
        }

        [UsedImplicitly]
        public Compound Union()
        {
            return new Compound(_components, CompoundType.Union);
        }

        [UsedImplicitly]
        public Compound UnionAll()
        {
            return new Compound(_components, CompoundType.UnionAll);
        }

        [UsedImplicitly]
        public Compound Intersects()
        {
            return new Compound(_components, CompoundType.Intersects);
        }

        [UsedImplicitly]
        public Compound Except()
        {
            return new Compound(_components, CompoundType.Except);
        }

        [UsedImplicitly]
        public OrderBy OrderBy()
        {
            return new OrderBy(_components);
        }

        [UsedImplicitly]
        public Limit Limit()
        {
            return new Limit(_components);
        }

        [UsedImplicitly]
        public Columns Columns()
        {
            return new Columns(_components);
        }

        [UsedImplicitly]
        public Joins InnerJoin()
        {
            return new Joins(_components).Inner();
        }

        [UsedImplicitly]
        public Joins LeftJoin()
        {
            return new Joins(_components).Left();
        }

        [UsedImplicitly]
        public Joins LeftOuterJoin()
        {
            return new Joins(_components).Left().Outer();
        }

        [UsedImplicitly]
        public Query Build() => _components.Build();
    }
}
