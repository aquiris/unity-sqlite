namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct IfNotExistsComponent : IQueryComponent
    {
        public string value => "IF NOT EXISTS";
    }
}