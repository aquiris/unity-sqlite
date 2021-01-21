namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct BindingComponent : IQueryComponent
    {
        public string value { get; }

        public BindingComponent(int index)
        {
            value = $"@binding{index}";
        }
    }
}