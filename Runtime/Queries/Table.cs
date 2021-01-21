using System;
using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public enum EditTableType
    {
        create,
        alter,
        drop,
    }
    
    public struct Table
    {
        private QueryComponents _components;

        internal Table(Table other)
        {
            _components = other._components;
        }

        internal Table(QueryComponents components)
        {
            _components = components;
        }

        public Table(EditTableType type, bool isView = false)
        {
            _components = new QueryComponents();
            switch (type)
            {
                case EditTableType.create:
                    StringComponent createTable = new StringComponent(Constants.QueryComponents.CREATE_TABLE);
                    StringComponent createView = new StringComponent(Constants.QueryComponents.CREATE_VIEW);
                    _components.Add(isView ? createView : createTable);
                    break;
                case EditTableType.alter:
                    if (isView) throw new NotSupportedException("Sqlite error: Cannot alter a view");
                    _components.Add(new StringComponent(Constants.QueryComponents.ALTER_TABLE));
                    break;
                case EditTableType.drop:
                    StringComponent dropTable = new StringComponent(Constants.QueryComponents.DROP_TABLE);
                    StringComponent dropView = new StringComponent(Constants.QueryComponents.DROP_VIEW);
                    _components.Add(isView ? dropView : dropTable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        [UsedImplicitly]
        public Table Temporary()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.TEMPORARY));
            return this;
        }

        [UsedImplicitly]
        public Table IfExists()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.IF_EXISTS));
            return this;
        }

        [UsedImplicitly]
        public Table IfNotExists()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.IF_NOT_EXISTS));
            return this;
        }

        [UsedImplicitly]
        public Table Name(string name)
        {
            _components.Add(new StringComponent(name));
            return this;
        }

        [UsedImplicitly]
        public Table RenameTo(string newName)
        {
            _components.Add(new StringComponent(Constants.QueryComponents.RENAME_TO));
            _components.Add(new StringComponent(newName));
            return this;
        }

        [UsedImplicitly]
        public As As()
        {
            return new As(_components);
        }

        [UsedImplicitly]
        public Columns AddColumn()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.ADD_COLUMN));
            return new Columns(_components);
        }

        [UsedImplicitly]
        public Columns Columns()
        {
            return new Columns(_components);
        }

        [UsedImplicitly]
        public Table WithoutRowId()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.WITHOUT_ROWID));
            return this;
        }

        [UsedImplicitly]
        public Query Build() => _components.Build();
    }
}