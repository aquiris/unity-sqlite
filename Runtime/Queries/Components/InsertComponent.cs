using System;
using Aquiris.SQLite.Shared;

namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct InsertComponent : IQueryComponent
    {
        public string value { get; }
        
        public InsertComponent(InsertMode mode)
        {
            switch (mode)
            {
                case InsertMode.Insert:
                    value = Constants.QueryComponents.INSERT;
                    break;
                case InsertMode.InsertOrAbort:
                    value = Constants.QueryComponents.INSERT_OR_ABORT;
                    break;
                case InsertMode.InsertOrFail:
                    value = Constants.QueryComponents.INSERT_OR_FAIL;
                    break;
                case InsertMode.InsertOrIgnore:
                    value = Constants.QueryComponents.INSERT_OR_IGNORE;
                    break;
                case InsertMode.InsertOrReplace:
                    value = Constants.QueryComponents.INSERT_OR_REPLACE;
                    break;
                case InsertMode.InsertOrRollback:
                    value = Constants.QueryComponents.INSERT_OR_ROLLBACK;
                    break;
                case InsertMode.Replace:
                    value = Constants.QueryComponents.REPLACE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}