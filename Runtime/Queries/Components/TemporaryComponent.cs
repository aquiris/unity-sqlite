namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct TemporaryComponent : IQueryComponent
    {
        public string value => "TEMPORARY";
    }
}