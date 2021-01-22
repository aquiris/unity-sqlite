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
                case InsertMode.insert:
                    value = Constants.QueryComponents.INSERT;
                    break;
                case InsertMode.insertOrAbort:
                    value = Constants.QueryComponents.INSERT_OR_ABORT;
                    break;
                case InsertMode.insertOrFail:
                    value = Constants.QueryComponents.INSERT_OR_FAIL;
                    break;
                case InsertMode.insertOrIgnore:
                    value = Constants.QueryComponents.INSERT_OR_IGNORE;
                    break;
                case InsertMode.insertOrReplace:
                    value = Constants.QueryComponents.INSERT_OR_REPLACE;
                    break;
                case InsertMode.insertOrRollback:
                    value = Constants.QueryComponents.INSERT_OR_ROLLBACK;
                    break;
                case InsertMode.replace:
                    value = Constants.QueryComponents.REPLACE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}