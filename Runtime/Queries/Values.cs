using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;
using static Aquiris.SQLite.Shared.Constants;

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
        public Values Begin()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.VALUES));
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
        public Values Add(string columnName, object value, bool addComma)
        {
            BindingComponent binding = new BindingComponent(columnName, _columnCount);
            _components.Add(binding);
            _components.AddBinding(binding.value, value);
            if (addComma) _components.Add(new StringComponent(Constants.QueryComponents.COMMA));
            _columnCount += 1;
            return this;
        }

        [UsedImplicitly]
        public Insert Insert()
        {
            return new Insert(_components);
        }
    }
}