using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct Values
    {
        private QueryComponents _components;
        private int _columnCount;
        
        internal Values(QueryComponents components)
        {
            _components = components;
            _columnCount = 0;
        }

        internal Values(Values other)
        {
            _components = other._components;
            _columnCount = other._columnCount;
        }

        [UsedImplicitly]
        public Values Begin(bool beginValues = true)
        {
            StringComponent values = new StringComponent(Constants.QueryComponents.VALUES);
            StringComponent comma = new StringComponent(Constants.QueryComponents.COMMA);
            _components.Add(beginValues ? values : comma);
            _components.Add(new StringComponent(Constants.QueryComponents.PARENTHESIS_OPEN));
            return this;
        }

        [UsedImplicitly]
        public Values End()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.PARENTHESIS_CLOSE));
            return this;
        }

        [UsedImplicitly]
        public Values Add(string columnName, object value)
        {
            BindingComponent binding = new BindingComponent(columnName, _columnCount);
            _components.Add(binding);
            _components.AddBinding(binding.value, value);
            _columnCount += 1;
            return this;
        }

        [UsedImplicitly]
        public Values Separator()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.COMMA));
            return this;
        }

        [UsedImplicitly]
        public Insert Insert()
        {
            return new Insert(_components);
        }
    }
}