﻿using UnityEngine;

namespace Aquiris.SQLite.Shared
{
    internal static class Constants
    {
        internal static class Colors
        {
            public static readonly Color lightBlue = new Color(.1F, .5F, 1F);
        }
        
        internal static class QueryComponents
        {
            public const string SPACE = " ";
            public const string COMMA = ",";
            public const string SEMICOLON = ";";
            public const string PARENTHESIS_OPEN = "(";
            public const string PARENTHESIS_CLOSE = ")";
            public const string SELECT_ALL = "*";

            public const string CREATE_TABLE = "CREATE TABLE";
            public const string TEMPORARY = "TEMPORARY";
            public const string TABLE = "TABLE";
            public const string IF = "IF";
            public const string NOT = "NOT";
            public const string TO = "TO";
            public const string IF_EXISTS = "IF EXISTS";
            public const string IF_NOT_EXISTS = "IF NOT EXISTS";
            public const string AS = "AS";
            public const string WITH = "WITH";
            public const string WITHOUT = "WITHOUT";
            public const string ROWID = "ROWID";
            public const string WITHOUT_ROWID = "WITHOUT ROWID";
            public const string SELECT = "SELECT";
            public const string INTO = "INTO";
            public const string OR = "OR";
            public const string REPLACE = "REPLACE";
            public const string RECURSIVE = "RECURSIVE";
            public const string INSERT = "INSERT";
            public const string ABORT = "ABORT";
            public const string FAIL = "FAIL";
            public const string IGNORE = "IGNORE";
            public const string ROLLBACK = "ROLLBACK";
            public const string INSERT_OR_ABORT = "INSERT OR ABORT";
            public const string INSERT_OR_FAIL = "INSERT OR FAIL";
            public const string INSERT_OR_IGNORE = "INSERT OR IGNORE";
            public const string INSERT_OR_REPLACE = "INSERT OR REPLACE";
            public const string INSERT_OR_ROLLBACK = "INSERT OR ROLLBACK";
            public const string VALUES = "VALUES";
            public const string DEFAULT = "DEFAULT";
            public const string DEFAULT_VALUES = "DEFAULT VALUES";
            public const string ALTER_TABLE = "ALTER TABLE";
            public const string RENAME_TO = "RENAME TO";
            public const string DROP_TABLE = "DROP TABLE";
            public const string ADD_COLUMN = "ADD COLUMN";
            public const string RENAME_COLUMN = "RENAME COLUMN";
            public const string DISTINCT = "DISTINCT";
            public const string FROM = "FROM";
            public const string WHERE = "WHERE";
            public const string GROUP_BY = "GROUP BY";
            public const string HAVING = "HAVING";
            public const string WINDOW = "WINDOW";
            public const string ALL = "ALL";
            public const string UNION = "UNION";
            public const string INTERSECT = "ITERSECT";
            public const string EXCEPT = "EXCEPT";
            public const string ORDER_BY = "ORDER_BY";
            public const string COLLATE = "COLLATE";
            public const string ASCENDING = "ASC";
            public const string DESCENDING = "DESC";
            public const string NULLS_FIRST = "NULLS FIRST";
            public const string NULLS_LAST = "NULLS LAST";
            public const string LIMIT = "LIMIT";
            public const string OFFSET = "OFFSET";
            public const string CREATE_VIEW = "CREATE VIEW";
            public const string DROP_VIEW = "DROP VIEW";
            public const string LEFT = "LEFT";
            public const string INNER = "INNER";
            public const string OUTER = "OUTER";
            public const string JOIN = "JOIN";
            public const string ON = "ON";
            public const string DELETE = "DELETE";
            public const string CONSTRAINT = "CONSTRAINT";
            public const string PRIMARY_KEY = "PRIMARY KEY";
            public const string ON_CONFLICT = "ON CONFLICT";
            public const string FOREIGN_KEY = "FOREIGN KEY";
            public const string CHECK = "CHECK";
            public const string SET_NULL = "SET_NULL";
            public const string SET_DEFAULT = "SET_DEFAULT";
            public const string CASCADE = "CASCADE";
            public const string RESTRICT = "RESTRICT";
            public const string NO_ACTION = "NO_ACTION";
            public const string UPDATE = "UPDATE";
            public const string REFERENCES = "REFERENCES";
            public const string MATCH = "MATCH";
            public const string DEFERRABLE = "DEFERRABLE";
            public const string INITIALLY_DEFERRED = "INITIALLY DEFERRED";
            public const string INITIALLY_IMMEDIATE = "INITIALLY IMMEDIATE";

            #region Operators

            public const string EQUAL = "==";
            public const string NOT_EQUAL = "!=";
            public const string GREATER = ">";
            public const string LESS = "<";
            public const string GREATER_OR_EQUAL = ">=";
            public const string LESS_OR_EQUAL = "<=";
            public const string NOT_GREATER = "!>";
            public const string NOT_LESS = "!<";

            #endregion

            #region Logical operators

            public const string AND = "AND";
            public const string BETWEEN = "BETWEEN";
            public const string EXISTS = "EXISTS";
            public const string IN = "IN";
            public const string NOT_IN = "NOT_IN";
            public const string LIKE = "LIKE";
            public const string GLOB = "GLOB";
            public const string IS_NULL = "IS NULL";
            public const string IS = "IS";
            public const string IS_NOT = "IS NOT";
            public const string APPEND = "||";
            public const string UNIQUE = "UNIQUE";

            #endregion
        }
    }
}
