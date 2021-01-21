namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct WithoutRowIdComponent : IQueryComponent
    {
        public string value => "WITHOUT ROWID";
    }
}