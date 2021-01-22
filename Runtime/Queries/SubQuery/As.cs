using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct As
    {
        private QueryComponents _components;
        
        internal As(QueryComponents components)
        {
            _components = components;
            _components.Add(new StringComponent(Constants.QueryComponents.AS));
        }

        [UsedImplicitly]
        public As Alias(string alias)
        {
            _components.Add(new StringComponent(alias));
            return this;
        }

        [UsedImplicitly]
        public Select Select()
        {
            return new Select(_components);
        }

        [UsedImplicitly]
        public Table BackToTable()
        {
            return new Table(_components);
        }

        [UsedImplicitly]
        public Insert BackToInsert()
        {
            return new Insert(_components);
        }
    }
}