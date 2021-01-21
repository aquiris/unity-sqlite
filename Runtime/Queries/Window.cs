using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct Window
    {
        private QueryComponents _components;

        internal Window(QueryComponents components)
        {
            _components = components;
            _components.Add(new StringComponent(Constants.QueryComponents.WINDOW));
        }

        [UsedImplicitly]
        public Window Name(string name, string definition, bool addComma)
        {
            _components.Add(new StringComponent(name));
            _components.Add(new StringComponent(Constants.QueryComponents.AS));
            _components.Add(new StringComponent(definition));
            if (addComma) _components.Add(new StringComponent(Constants.QueryComponents.COMMA));
            return this;
        }

        [UsedImplicitly]
        public Select Select()
        {
            return new Select(_components);
        } 
    }
}