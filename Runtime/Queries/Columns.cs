using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct Columns
    {
        private QueryComponents _components;

        internal Columns(Columns other)
        {
            _components = other._components;
        }
        
        internal Columns(QueryComponents components)
        {
            _components = components;
        }

        /// <summary>
        /// Adds an opening parenthesis before defining columns.
        /// Should be used in CREATE TABLE columns statement for example.
        /// </summary>
        /// <returns>A copy of <see cref="Columns"/></returns>
        [UsedImplicitly]
        public Columns Begin()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.PARENTHESIS_OPEN));
            return this;
        }
        
        /// <summary>
        /// Adds an closing parenthesis after defining columns.
        /// Should be used in CREATE TABLE columns statement for example.
        /// </summary>
        /// <returns></returns>
        [UsedImplicitly]
        public Columns End()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.PARENTHESIS_CLOSE));
            return this;
        }

        [UsedImplicitly]
        public Columns AddColumn(string name)
        {
            _components.Add(new ColumnDefinitionComponent(name));
            return this;
        }

        [UsedImplicitly]
        public Columns Separator()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.COMMA));
            return this;
        }

        [UsedImplicitly]
        public Columns DeclareColumn(string name, DataType type, bool addComma)
        {
            _components.Add(new ColumnDefinitionComponent(name, type));
            if (addComma) _components.Add(new StringComponent(Constants.QueryComponents.COMMA));
            return this;
        }

        [UsedImplicitly]
        public Columns Rename(string name, string newName)
        {
            _components.Add(new StringComponent(Constants.QueryComponents.RENAME_COLUMN));
            _components.Add(new StringComponent(name));
            _components.Add(new StringComponent(Constants.QueryComponents.TO));
            _components.Add(new StringComponent(newName));
            return this;
        }

        [UsedImplicitly]
        public As As()
        {
            return new As(_components);
        }

        [UsedImplicitly]
        public Table Table()
        {
            return new Table(_components);
        }

        [UsedImplicitly]
        public Insert Insert()
        {
            return new Insert(_components);
        }

        [UsedImplicitly]
        public Select Select()
        {
            return new Select(_components);
        }
    }
}