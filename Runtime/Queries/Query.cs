using System.Collections.Generic;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct Query
    {
        public string statement;

        public KeyValuePair<string, object>[] bindings { get; private set; }
        public int bindingsCount { get; private set; }

        public Query(string statement)
        {
            this.statement = statement;
            bindingsCount = 0;
            bindings = new KeyValuePair<string, object>[Constants.maxNumberOfBindings];
        }

        [UsedImplicitly]
        public void Add(KeyValuePair<string, object> binding)
        {
            if (bindings == null) bindings = new KeyValuePair<string, object>[Constants.maxNumberOfBindings];
            bindings[bindingsCount] = binding;
            bindingsCount += 1;
        }

        [UsedImplicitly]
        public void Add(string column, object value) => Add(new KeyValuePair<string, object>(column, value));

        [UsedImplicitly]
        public void Add(KeyValuePair<string, object>[] collection, int count)
        {
            for (int index = 0; index < count; index++)
            {
                Add(collection[count]);
            }
        }
    }
}