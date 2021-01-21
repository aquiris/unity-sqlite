namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct SemiColonComponent : IQueryComponent
    {
        public string value => ";";
    }
}