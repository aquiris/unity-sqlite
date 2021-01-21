using System.Collections.Generic;
using Aquiris.SQLite.Shared;
using Aquiris.SQLite.Tables;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public struct SQLiteTable : IQueryComponent
    {
        private static readonly SQLiteColumn[] _columnsBuffer = new SQLiteColumn[Constants.maxNumberOfBindings];

        private readonly string _name;
        private readonly bool _temporary;
        private SQLiteQueryComponents _components;
        private bool _ifNotExists;
        private bool _withoutRowId;
        private SQLiteAs? _asSelect;
        private int _columnCount;

        public KeyValuePair<string, object>[] bindings { get; }
        public int bindingCount { get; }

        internal SQLiteTable(SQLiteQueryComponents components, string name, bool temporary = false) : this()
        {
            _components = components;
            _name = name;
            _temporary = temporary;
            _ifNotExists = false;
        }

        [UsedImplicitly]
        public SQLiteTable IfNotExists()
        {
            _ifNotExists = true;
            return this;
        }

        [UsedImplicitly]
        public SQLiteTable As(SQLiteSelect select)
        {
            _asSelect = new SQLiteAs(select);
            return this;
        }

        [UsedImplicitly]
        public SQLiteTable AddColumn(SQLiteColumn column)
        {
            _columnsBuffer[_columnCount] = column;
            _columnCount += 1;
            return this;
        }

        [UsedImplicitly]
        public SQLiteTable WithoutRowId()
        {
            _withoutRowId = true;
            return this;
        }

        public SQLiteQuery Finish()
        {
            _components.Add(this);
            return _components.Build();
        }

        public string Build()
        {
            string statement;
            switch (_temporary)
            {
                case true when _ifNotExists:
                    statement = $"TEMPORARY TABLE IF NOT EXISTS {_name}";
                    break;
                case true:
                    statement = $"TEMPORARY TABLE {_name}";
                    break;
                default:
                    statement = $"TABLE IF NOT EXISTS {_name}";
                    break;
            }
            if (_asSelect.HasValue) statement += $" {_asSelect.Value.Build()}";
            statement += $" {SQLiteColumn.GetCreateTableColumnsStatement(_columnsBuffer, _columnCount)}";
            if (_withoutRowId) statement += " WITHOUT ROWID";
            return statement;
        }
    }
}