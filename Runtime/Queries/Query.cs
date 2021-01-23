using System.Collections.Generic;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct Query
    {
        public string statement;

        public List<KeyValuePair<string, object>> bindings { get; private set; }
        public int bindingCount => bindings?.Count ?? 0;
        
        public Query(string statement)
        {
            this.statement = statement;
            bindings = new List<KeyValuePair<string, object>>();
        }

        [UsedImplicitly]
        public void Bind(KeyValuePair<string, object> binding)
        {
            if (bindings == null) bindings = new List<KeyValuePair<string, object>>();
            if (Find(binding.Key, out int bindingIndex))
            {
                bindings[bindingIndex] = binding;
                return;
            }
            bindings.Add(binding);
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

        private bool Find(string binding, out int existingIndex)
        {
            for (int index = 0; index < bindings.Count; index++)
            {
                if (!string.Equals(binding, bindings[index].Key)) continue;
                existingIndex = index;
                return true;
            }
            existingIndex = -1;
            return false;
        }
    }
}