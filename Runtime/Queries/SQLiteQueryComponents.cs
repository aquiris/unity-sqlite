using System.Text;
using Aquiris.SQLite.Shared;

namespace Aquiris.SQLite.Queries
{
    internal struct SQLiteQueryComponents
    {
        private static readonly StringBuilder _builder = new StringBuilder();
        private static readonly IQueryComponent[] _components = new IQueryComponent[Constants.maxNumberOfQueryComponents];
        private int _count;

        public void Add(IQueryComponent component)
        {
            _components[_count] = component;
            _count += 1;
        }

        public SQLiteQuery Build()
        {
            _builder.Clear();
            SQLiteQuery query = new SQLiteQuery();
            for (int index = 0; index < _count; index++)
            {
                IQueryComponent component = _components[index];
                _builder.Append(component.Build());
                query.Add(component.bindings, component.bindingCount);

                if (index < _count - 1)
                {
                    _builder.Append(" ");
                }
            }
            _builder.Append(";");
            query.statement = _builder.ToString();
            return query;
        }
    }
}