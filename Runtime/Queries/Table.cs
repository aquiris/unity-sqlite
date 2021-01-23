using System;
using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public enum TableMode
    {
        Create,
        Alter,
        Drop,
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
        
        [UsedImplicitly]
        public Table Begin(TableMode mode, bool isView = false)
        {
            switch (mode)
            {
                case TableMode.Create:
                    StringComponent createTable = new StringComponent(Constants.QueryComponents.CREATE_TABLE);
                    StringComponent createView = new StringComponent(Constants.QueryComponents.CREATE_VIEW);
                    _components.Add(isView ? createView : createTable);
                    break;
                case TableMode.Alter:
                    if (isView) throw new NotSupportedException("Sqlite error: Cannot alter a view");
                    _components.Add(new StringComponent(Constants.QueryComponents.ALTER_TABLE));
                    break;
                case TableMode.Drop:
                    StringComponent dropTable = new StringComponent(Constants.QueryComponents.DROP_TABLE);
                    StringComponent dropView = new StringComponent(Constants.QueryComponents.DROP_VIEW);
                    _components.Add(isView ? dropView : dropTable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
            return this;
        }

        [UsedImplicitly]
        public Table End()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.SEMICOLON));
            return this;
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