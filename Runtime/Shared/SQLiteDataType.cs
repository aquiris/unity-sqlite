using System;

namespace Aquiris.SQLite.Shared
{
    public enum SQLiteDataType
    {
        Integer = 1,
        Text = 2,
        Blob = 3,
        Real = 4,
        Numeric = 5
    }

    public static class SQLiteDataTypeExt
    {
        public static string Convert(this SQLiteDataType dataType)
        {
            switch (dataType)
            {
                case SQLiteDataType.Blob: return "BLOB";
                case SQLiteDataType.Integer: return "INTEGER";
                case SQLiteDataType.Numeric: return "NUMERIC";
                case SQLiteDataType.Real: return "REAL";
                case SQLiteDataType.Text: return "TEXT";
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
        }
    }
}
