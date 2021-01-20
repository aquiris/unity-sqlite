using System.IO;
using UnityEngine;

namespace Aquiris.SQLite.Tests.Constants
{
    public static class Constants
    {
        public static readonly string databaseParentPath = Path.Combine(Application.dataPath, "SQLite");
        public static readonly string databaseFilePath = Path.Combine(databaseParentPath, "database.db");
    }
}