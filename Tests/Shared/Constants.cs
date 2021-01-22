using System.IO;
using UnityEngine;

namespace Aquiris.SQLite.Tests.Shared
{
    public static class Constants
    {
        public static readonly string databaseParentPath = Path.Combine(Application.dataPath, "SQLite");
        public static readonly string databaseFilePath = Path.Combine(databaseParentPath, "database.db");

        // will wait util the value is Set
        public const int waitTimeOut = -1; 
    }
}
