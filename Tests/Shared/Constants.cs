using System.IO;
using UnityEngine;

namespace Aquiris.SQLite.Tests.Shared
{
    public static class Constants
    {
        public static readonly string databaseParentPath = Path.Combine(Application.dataPath, "SQLite");
        public static readonly string databaseFilePath = Path.Combine(databaseParentPath, "database.db");
        
        // if this timeout is overdue, its taking
        // too much time to execute something
        public const int waitTimeOut = 2000; 
    }
}
