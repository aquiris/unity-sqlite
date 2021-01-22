using System.Collections.Generic;
using System.Text;
using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    internal struct QueryComponents
    {
        private static readonly StringBuilder _builder = new StringBuilder();
        private static readonly IQueryComponent[] _components = new IQueryComponent[Constants.maxNumberOfQueryComponents];
        private Query _query;
        private int _count;

        [UsedImplicitly]
        public QueryComponents(QueryComponents other)
        {
            _query = other._query;
            _count = other._count;
        }

        [UsedImplicitly]
        public void Add(IQueryComponent component)
        {
            _components[_count] = component;
            _count += 1;
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
            
            for (int index = 0; index < _count; index++)
            {
                IQueryComponent component = _components[index];
                _builder.Append(component.value);
                if (index < _count - 1)
                {
                    _builder.Append(" ");
                }
            }
            _query.statement = _builder.ToString();
            return _query;
        }
    }
}