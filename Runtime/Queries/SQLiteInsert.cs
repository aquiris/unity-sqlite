using System;
using System.Collections.Generic;

namespace Aquiris.SQLite.Queries
{
    public enum SQLiteInsertType
    {
        insert,
        insertOrAbort,
        insertOrFail,
        insertOrIgnore,
        insertOrReplace,
        insertOrRollback,
        replace,
    }
    
    public struct SQLiteInsert 
    {
        private SQLiteQueryComponents _components;
        private SQLiteInsertType _type;
        private SQLiteTable _table;
        private string _asAlias;
        
        public KeyValuePair<string, object>[] bindings { get; }
        public int bindingCount { get; }

        public SQLiteInsert(SQLiteInsertType type) : this()
        {
            _type = type;
        }

        public SQLiteInsert Into(SQLiteTable table)
        {
            _table = table;
            return this;
        }

        public SQLiteInsert As(string alias)
        {
            _asAlias = alias;
            return this;
        }

        public SQLiteInsert AddColumn(string column, object value)
        {
            
        }
        
        public SQLiteQuery Finish()
        {
            throw new System.NotImplementedException();
        }

        public string Build()
        {
            switch (_type)
            {
                case SQLiteInsertType.insert: return "INSERT INTO";
                case SQLiteInsertType.insertOrAbort: return "INSERT OR ABORT INTO";
                case SQLiteInsertType.insertOrFail: return "INSERT OR FAIL INTO";
                case SQLiteInsertType.insertOrIgnore: return "INSERT OR IGNORE INTO";
                case SQLiteInsertType.insertOrReplace: return "INSERT OR REPLACE INTO";
                case SQLiteInsertType.insertOrRollback: return "INSERT OR ROLLBACK INTO";
                case SQLiteInsertType.replace: return "REPLACE INTO";
                default: throw new ArgumentOutOfRangeException(nameof(_type), _type, null);
            }
        }
    }
}