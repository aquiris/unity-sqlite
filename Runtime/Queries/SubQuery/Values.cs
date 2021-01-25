using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct Values
    {
        private QueryComponents _components;

        internal Values(QueryComponents components)
        {
            _components = components;
        }

        internal Values(Values other)
        {
            _components = other._components;
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
        public Values Bind(object value)
        {
            BindingComponent binding = BindingComponent.Binding();
            _components.Add(binding);
            _components.AddBinding(binding.value, value);
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
