using System.IO;
using NUnit.Framework;

namespace Aquiris.SQLite.Tests.Shared
{
    public abstract class BaseTestClass
    {
        [SetUp]
        public virtual void SetUp()
        {
            if (!File.Exists(Constants.databaseFilePath)) return;
            File.Delete(Constants.databaseFilePath);
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (!Directory.Exists(Constants.databaseParentPath)) return;
            Directory.Delete(Constants.databaseParentPath, true);
        }
    }
}
