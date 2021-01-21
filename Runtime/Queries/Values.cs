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
            _components.Add(new StringComponent(Constants.QueryComponents.PARENTHESIS_OPEN));
        }

        internal Values(Values other)
        {
            _components = other._components;
            _columnCount = other._columnCount;
        }

        [UsedImplicitly]
        public Values Add(object value)
        {
            BindingComponent binding = new BindingComponent(_columnCount);
            _components.AddBinding(binding.value, value);
            _components.Add(binding);
            _columnCount += 1;
            return this;
        }

        [UsedImplicitly]
        public Insert BackToInsert()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.PARENTHESIS_CLOSE));
            return new Insert(_components);
        }
    }
}