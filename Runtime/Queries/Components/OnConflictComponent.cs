using System;
using Aquiris.SQLite.Shared;

namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct OnConflictComponent : IQueryComponent
    {
        public string value { get; }

        public OnConflictComponent(ConflictMode mode)
        {
            value = Constants.QueryComponents.ON_CONFLICT + Constants.QueryComponents.SPACE;
            switch (mode)
            {
                case ConflictMode.Abort:
                    value += Constants.QueryComponents.ABORT;
                    break;
                case ConflictMode.Fail:
                    value += Constants.QueryComponents.FAIL;
                    break;
                case ConflictMode.Ignore:
                    value += Constants.QueryComponents.IGNORE;
                    break;
                case ConflictMode.Replace:
                    value += Constants.QueryComponents.REPLACE;
                    break;
                case ConflictMode.Rollback:
                    value += Constants.QueryComponents.ROLLBACK;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}
