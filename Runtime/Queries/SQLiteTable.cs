using Aquiris.SQLite.Queries.Components;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct SQLiteTable
    {
        private SQLiteQueryComponents _components;

        internal SQLiteTable(SQLiteTable other)
        {
            _components = other._components;
        }

        internal SQLiteTable(SQLiteQueryComponents components)
        {
            _components = components;
        }
        
        [UsedImplicitly]
        public static SQLiteTable Create()
        {
            SQLiteTable table = new SQLiteTable();
            table._components.Add(new CreateComponent());
            return table;
        }

        [UsedImplicitly]
        public SQLiteTable Temporary()
        {
            _components.Add(new TemporaryComponent());
            return this;
        }

        [UsedImplicitly]
        public SQLiteTable IfNotExists()
        {
            _components.Add(new IfNotExistsComponent());
            return this;
        }

        [UsedImplicitly]
        public SQLiteTable TableName(string name)
        {
            _components.Add(new TableNameComponent(name));
            return this;
        }

        [UsedImplicitly]
        public SQLiteSelectAs As()
        {
            return new SQLiteSelectAs(_components);
        }

        [UsedImplicitly]
        public SQLiteColumn DefineColumns()
        {
            return new SQLiteColumn(_components);
        }

        [UsedImplicitly]
        public SQLiteTable WithoutRowId()
        {
            _components.Add(new WithoutRowIdComponent());
            return this;
        }

        [UsedImplicitly]
        public SQLiteQuery Build() => _components.Build();
    }
}