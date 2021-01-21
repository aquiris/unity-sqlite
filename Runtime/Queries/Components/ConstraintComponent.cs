namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct ConstraintComponent : IQueryComponent
    {
        public string value { get; }

        public ConstraintComponent(string name)
        {
            value = $"CONSTRAINT {name}";
        }
    }
}