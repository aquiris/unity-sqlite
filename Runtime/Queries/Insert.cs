using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public enum InsertMode
    {
        insert,
        insertOrAbort,
        insertOrFail,
        insertOrIgnore,
        insertOrReplace,
        insertOrRollback,
        replace,
    }
    
    public struct Insert 
    {
        private QueryComponents _components;

        internal Insert(Insert other)
        {
            _components = other._components;
        }

        internal Insert(QueryComponents components)
        {
            _components = components;
        }
        
        [UsedImplicitly]
        public Insert Begin(InsertMode mode)
        {
            _components.Add(new InsertComponent(mode));
            return this;
        }

        [UsedImplicitly]
        public Insert End()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.SEMICOLON));
            return this;
        }

        [UsedImplicitly]
        public Insert IntoTable(string name)
        {
            _components.Add(new StringComponent(Constants.QueryComponents.INTO));
            _components.Add(new StringComponent(name));
            return this;
        }

        [UsedImplicitly]
        public As As()
        {
            return new As(_components);
        }
        
        [UsedImplicitly]
        public Insert DefaultValues()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.DEFAULT_VALUES));
            return this;
        }

        [UsedImplicitly]
        public Columns Columns()
        {
            return new Columns(_components);
        }

        [UsedImplicitly]
        public Values Values()
        {
            return new Values(_components);
        }

        [UsedImplicitly]
        public Select Select()
        {
            return new Select(_components);
        }

        [UsedImplicitly]
        public Query Build() => _components.Build();
    }
}