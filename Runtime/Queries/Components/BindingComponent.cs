namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct BindingComponent : IQueryComponent
    {
        public string value { get; }

        public BindingComponent(string columnName, int index)
        {
            value = $"@{columnName}{index}";
        }
    }
}