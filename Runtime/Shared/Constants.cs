using System;

namespace Aquiris.SQLite.Shared
{
    internal static class Constants
    {
        public static readonly string newLine = Environment.NewLine;
        public static readonly string commaNewLine = $", {newLine}";

        public const int maxNumberOfBindings = 100;
        public const int maxNumberOfQueryComponents = 100;
        public const int maxNumberOfColumns = 100;
        public const int maxNumberOfQueries = 1024 * 1024;
    }
}