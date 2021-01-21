namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct ParenthesisComponent : IQueryComponent
    {
        public string value { get; }

        public ParenthesisComponent(bool open)
        {
            value = open ? "(" : ")";
        }
    }
}