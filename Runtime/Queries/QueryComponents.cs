using System.Collections.Generic;
using System.Text;
using Aquiris.SQLite.Queries.Components;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    internal struct QueryComponents
    {
        private static readonly StringBuilder _builder = new StringBuilder();
        private static readonly List<IQueryComponent> _components = new List<IQueryComponent>();
        
        private Query _query;
        
        [UsedImplicitly]
        public QueryComponents(QueryComponents other)
        {
            _query = other._query;
        }

        [UsedImplicitly]
        public void Add(IQueryComponent component)
        {
            _components.Add(component);
        }

        [UsedImplicitly]
        public void AddBinding(string key, object value)
        {
            _query.Bind(key, value);
        }

        [UsedImplicitly]
        public void AddBinding(KeyValuePair<string, object> binding)
        {
            _query.Bind(binding);
        }

        [UsedImplicitly]
        public void AddBinding(KeyValuePair<string, object>[] bindings, int count)
        {
            _query.Bind(bindings, count);
        }

        [UsedImplicitly]
        public Query Build()
        {
            _builder.Clear();
            int componentsCount = _components.Count;
            for (int index = 0; index < componentsCount; index++)
            {
                IQueryComponent component = _components[index];
                _builder.Append(component.value);
                if (index < componentsCount - 1) _builder.Append(" ");
            }
            _components.Clear();
            _query.statement = _builder.ToString();
            return _query;
        }
    }
}
