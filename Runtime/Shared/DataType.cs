using System;

namespace Aquiris.SQLite.Shared
{
    public enum DataType
    {
        Integer = 1,
        Text = 2,
        Blob = 3,
        Real = 4,
        Numeric = 5
    }

    public static class DataTypeExt
    {
        public static string Convert(this DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Blob: return "BLOB";
                case DataType.Integer: return "INTEGER";
                case DataType.Numeric: return "NUMERIC";
                case DataType.Real: return "REAL";
                case DataType.Text: return "TEXT";
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
        }
    }
}
