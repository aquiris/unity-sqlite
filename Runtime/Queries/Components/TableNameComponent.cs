namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct TableNameComponent : IQueryComponent
    {
        public string value { get; }

        public TableNameComponent(string name)
        {
            value = name;
        }
    }
}