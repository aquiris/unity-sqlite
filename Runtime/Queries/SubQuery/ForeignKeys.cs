using System;
using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public enum ForeignKeyOnMode
    {
        Delete,
        Update,
    }

    public enum ForeignKeyAction
    {
        SetNull,
        SetDefault,
        Cascade,
        Restrict,
        NoAction,
    }
    
    public struct ForeignKeys
    {
        private QueryComponents _components;

        internal ForeignKeys(QueryComponents components)
        {
            _components = components;
        }

        [UsedImplicitly]
        public ForeignKeys Begin()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.FOREIGN_KEY));
            return this;
        }

        [UsedImplicitly]
        public ForeignKeys References(string table)
        {
            _components.Add(new StringComponent(Constants.QueryComponents.REFERENCES));
            _components.Add(new StringComponent(table));
            return this;
        }

        [UsedImplicitly]
        public ForeignKeys On(ForeignKeyOnMode mode, ForeignKeyAction action)
        {
            _components.Add(new StringComponent(Constants.QueryComponents.ON));
            switch (mode)
            {
                case ForeignKeyOnMode.Delete:
                    _components.Add(new StringComponent(Constants.QueryComponents.DELETE));
                    break;
                case ForeignKeyOnMode.Update:
                    _components.Add(new StringComponent(Constants.QueryComponents.UPDATE));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            switch (action)
            {
                case ForeignKeyAction.SetNull:
                    _components.Add(new StringComponent(Constants.QueryComponents.SET_NULL));
                    break;
                case ForeignKeyAction.SetDefault:
                    _components.Add(new StringComponent(Constants.QueryComponents.SET_DEFAULT));
                    break;
                case ForeignKeyAction.Cascade:
                    _components.Add(new StringComponent(Constants.QueryComponents.CASCADE));
                    break;
                case ForeignKeyAction.Restrict:
                    _components.Add(new StringComponent(Constants.QueryComponents.RESTRICT));
                    break;
                case ForeignKeyAction.NoAction:
                    _components.Add(new StringComponent(Constants.QueryComponents.NO_ACTION));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
            return this;
        }

        [UsedImplicitly]
        public ForeignKeys Match(string name)
        {
            _components.Add(new StringComponent(Constants.QueryComponents.MATCH));
            _components.Add(new StringComponent(name));
            return this;
        }

        [UsedImplicitly]
        public ForeignKeys Not()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.NOT));
            return this;
        }

        [UsedImplicitly]
        public ForeignKeys Deferrable()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.DEFERRABLE));
            return this;
        }

        [UsedImplicitly]
        public ForeignKeys InitiallyDeferred()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.INITIALLY_DEFERRED));
            return this;
        }

        [UsedImplicitly]
        public ForeignKeys InitiallyImmediate()
        {
            _components.Add(new StringComponent(Constants.QueryComponents.INITIALLY_IMMEDIATE));
            return this;
        }

        [UsedImplicitly]
        public Columns Columns()
        {
            return new Columns(_components);
        }
    }
}