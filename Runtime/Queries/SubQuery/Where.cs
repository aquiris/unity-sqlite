using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct Where
    {
        private QueryComponents _components;

        internal Where(QueryComponents components)
        {
            _components = components;
            _components.Add(new StringComponent(Constants.QueryComponents.WHERE));
        }

        [UsedImplicitly]
        public Where Column(string columnName)
        {
            _components.Add(new StringComponent(columnName));
            return this;
        }

        [UsedImplicitly]
        public Where Binding(object value)
        {
            BindingComponent binding = BindingComponent.Binding();
            _components.AddBinding(binding.value, value);
            _components.Add(binding);
            return this;
        }

        [UsedImplicitly]
        public Where Equal()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.EQUAL));
            return this;
        }

        [UsedImplicitly]
        public Where NotEqual()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.NOT_EQUAL));
            return this;
        }

        [UsedImplicitly]
        public Where Greater()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.GREATER));
            return this;
        }

        [UsedImplicitly]
        public Where GreaterOrEqual()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.GREATER_OR_EQUAL));
            return this;
        }

        [UsedImplicitly]
        public Where Less()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.LESS));
            return this;
        }

        [UsedImplicitly]
        public Where LessOrEqual()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.LESS_OR_EQUAL));
            return this;
        }

        [UsedImplicitly]
        public Where NotGreater()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.NOT_GREATER));
            return this;
        }

        [UsedImplicitly]
        public Where NotLess()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.NOT_LESS));
            return this;
        }
        
        [UsedImplicitly]
        public Where And()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.AND));
            return this;
        }

        [UsedImplicitly]
        public Where Or()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.OR));
            return this;
        }

        [UsedImplicitly]
        public Where Between()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.BETWEEN));
            return this;
        }

        [UsedImplicitly]
        public Where Exists()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.EXISTS));
            return this;
        }

        [UsedImplicitly]
        public Where In()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.IN));
            return this;
        }

        [UsedImplicitly]
        public Where NotIn()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.NOT_IN));
            return this;
        }

        [UsedImplicitly]
        public Where Like()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.LIKE));
            return this;
        }

        [UsedImplicitly]
        public Where Glob()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.GLOB));
            return this;
        }

        [UsedImplicitly]
        public Where Not()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.NOT));
            return this;
        }

        [UsedImplicitly]
        public Where IsNull()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.IS_NULL));
            return this;
        }

        [UsedImplicitly]
        public Where Is()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.IS));
            return this;
        }

        [UsedImplicitly]
        public Where IsNot()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.IS_NOT));
            return this;
        }

        /// <summary>
        /// Adds two string into a new one.
        /// </summary>
        /// <returns>A copy of <see cref="Where"/></returns>
        [UsedImplicitly]
        public Where Append()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.APPEND));
            return this;
        }
        
        [UsedImplicitly]
        public Select Select()
        {
            return new Select(_components);
        }
        
        [UsedImplicitly]
        public GroupBy GroupBy()
        {
            return new GroupBy(_components);
        }

        [UsedImplicitly]
        public Delete Delete()
        {
            return new Delete(_components);
        }
    }
}