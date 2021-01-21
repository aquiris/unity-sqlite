using System;
using Aquiris.SQLite.Shared;

namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct InsertComponent : IQueryComponent
    {
        public string value { get; }
        
        public InsertComponent(InsertType type)
        {
            switch (type)
            {
                case InsertType.insert:
                    value = Constants.QueryComponents.INSERT;
                    break;
                case InsertType.insertOrAbort:
                    value = Constants.QueryComponents.INSERT_OR_ABORT;
                    break;
                case InsertType.insertOrFail:
                    value = Constants.QueryComponents.INSERT_OR_FAIL;
                    break;
                case InsertType.insertOrIgnore:
                    value = Constants.QueryComponents.INSERT_OR_IGNORE;
                    break;
                case InsertType.insertOrReplace:
                    value = Constants.QueryComponents.INSERT_OR_REPLACE;
                    break;
                case InsertType.insertOrRollback:
                    value = Constants.QueryComponents.INSERT_OR_ROLLBACK;
                    break;
                case InsertType.replace:
                    value = Constants.QueryComponents.REPLACE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}