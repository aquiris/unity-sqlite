namespace Aquiris.SQLite.Queries
{
    public struct SQLiteSelect : IQueryComponent
    {
        private SQLiteQueryComponents _components;

        internal SQLiteSelect(SQLiteQueryComponents components)
        {
            _components = components;
        }
    }
}