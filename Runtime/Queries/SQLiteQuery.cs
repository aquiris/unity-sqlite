using System.Collections.Generic;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct SQLiteQuery
    {
        private static readonly KeyValuePair<string, object>[] _bindingsBuffer = new KeyValuePair<string, object>[Constants.maxNumberOfBindings]; 

        public string statement;
        
        public int bindingsCount { get; private set; }
        public IReadOnlyList<KeyValuePair<string, object>> bindings => _bindingsBuffer;

        public SQLiteQuery(string statement)
        {
            this.statement = statement;
            bindingsCount = 0;
        }

        [UsedImplicitly]
        public void Add(KeyValuePair<string, object> binding)
        {
            _bindingsBuffer[bindingsCount] = binding;
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