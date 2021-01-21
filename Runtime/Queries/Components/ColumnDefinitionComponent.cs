using Aquiris.SQLite.Shared;

namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct ColumnDefinitionComponent : IQueryComponent
    {
        public string value { get; }

        public ColumnDefinitionComponent(string name)
        {
            value = name;
        }

        public ColumnDefinitionComponent(string name, SQLiteDataType type)
        {
            value = $"{name} {type.Convert()}";
        }
    }
}