using System.Collections.Generic;
using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;
using static Aquiris.SQLite.Shared.Constants;

namespace Aquiris.SQLite.Queries
{
    public struct Select
    {
        private QueryComponents _components;

        internal Select(QueryComponents components)
        {
            _components = components;
            _components.Add(new StringComponent(Constants.QueryComponents.SELECT));
        }

        [UsedImplicitly]
        public Columns DefineColumns()
        {
            return new Columns(_components);
        }

        [UsedImplicitly]
        public Query Build() => _components.Build();
    }
}