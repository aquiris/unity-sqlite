namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct StringComponent : IQueryComponent
    {
        public string value { get; }

        public StringComponent(string value)
        {
            this.value = value;
        }
    }
}