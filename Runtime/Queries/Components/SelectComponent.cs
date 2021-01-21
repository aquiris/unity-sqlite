namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct SelectComponent : IQueryComponent
    {
        public string value => "SELECT";
    }
}