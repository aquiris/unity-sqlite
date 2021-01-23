using System;
using Aquiris.SQLite.Shared;

namespace Aquiris.SQLite.Queries.Components
{
    internal readonly struct InsertComponent : IQueryComponent
    {
        public string value { get; }

        public InsertComponent(InsertMode insertMode)
        {
            switch (insertMode)
            {
                case InsertMode.Insert:
                    value = Constants.QueryComponents.INSERT;
                    break;
                case InsertMode.Replace:
                    value = Constants.QueryComponents.REPLACE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(insertMode), insertMode, null);
            }
        }

        public InsertComponent(ConflictMode conflictMode)
        {
            switch (conflictMode)
            {
                case ConflictMode.Abort:
                    value = Constants.QueryComponents.INSERT_OR_ABORT;
                    break;
                case ConflictMode.Fail:
                    value = Constants.QueryComponents.INSERT_OR_FAIL;
                    break;
                case ConflictMode.Ignore:
                    value = Constants.QueryComponents.INSERT_OR_IGNORE;
                    break;
                case ConflictMode.Replace:
                    value = Constants.QueryComponents.INSERT_OR_REPLACE;
                    break;
                case ConflictMode.Rollback:
                    value = Constants.QueryComponents.INSERT_OR_ROLLBACK;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(conflictMode), conflictMode, "Unexpected value");
            }
        }
    }
}