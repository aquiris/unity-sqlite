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
        public void Bind(KeyValuePair<string, object> binding)
        {
            if (bindings == null) bindings = new KeyValuePair<string, object>[Constants.maxNumberOfBindings];
            int bindingIndex = bindingsCount;
            if (!Find(binding.Key, ref bindingIndex))
                bindingsCount += 1;
            bindings[bindingIndex] = binding;
        }

        [UsedImplicitly]
        public void Bind(string column, object value) => Bind(new KeyValuePair<string, object>(column, value));

        [UsedImplicitly]
        public void Bind(KeyValuePair<string, object>[] collection, int count)
        {
            for (int index = 0; index < count; index++)
            {
                Bind(collection[count]);
            }
        }

        private bool Find(string binding, ref int existingIndex)
        {
            for (int index = 0; index < bindingsCount; index++)
            {
                if (!string.Equals(binding, bindings[index].Key)) continue;
                existingIndex = index;
                return true;
            }
            return false;
        }
    }
}