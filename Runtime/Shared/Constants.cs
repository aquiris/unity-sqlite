using System;

namespace Aquiris.SQLite.Shared
{
    internal static class Constants
    {
        public static readonly string newLine = Environment.NewLine;
        public static readonly string commaNewLine = $", {newLine}";

        public const int maxNumberOfBindings = 100;
        public const int maxNumberOfQueryComponents = 1024;
        public const int maxNumberOfColumns = 100;
        public const int maxNumberOfQueries = 1024 * 1024;

        internal static class QueryComponents
        {
            public const string SPACE = " ";
            public const string COMMA = ",";
            public const string SEMICOLON = ";";
            public const string PARENTHESIS_OPEN = "(";
            public const string PARENTHESIS_CLOSE = ")";
            public const string CREATE = "CREATE";
            public const string TEMPORARY = "TEMPORARY";
            public const string TABLE = "TABLE";
            public const string IF = "IF";
            public const string NOT = "NOT";
            public const string EXISTS = "EXISTS";
            public const string IF_NOT_EXISTS = "IF NOT EXISTS";
            public const string AS = "AS";
            public const string WITHOUT = "WITHOUT";
            public const string ROWID = "ROWID";
            public const string WITHOUT_ROWID = "WITHOUT ROWID";
            public const string SELECT = "SELECT";
            public const string INTO = "INTO";
            public const string OR = "OR";
            public const string REPLACE = "REPLACE";
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
        }
    }
}