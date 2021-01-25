using System;
using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public enum CompoundType
    {
        Union,
        UnionAll,
        Intersects,
        Except,
    }
    
    public readonly struct Compound
    {
        private readonly QueryComponents _components;

        internal Compound(QueryComponents components, CompoundType type)
        {
            _components = components;
            switch (type)
            {
                case CompoundType.Union:
                    _components.Add(new StringComponent(Constants.QueryComponents.UNION));
                    break;
                case CompoundType.UnionAll:
                    _components.Add(new StringComponent(Constants.QueryComponents.UNION));
                    _components.Add(new StringComponent(Constants.QueryComponents.SELECT_ALL));
                    break;
                case CompoundType.Intersects:
                    _components.Add(new StringComponent(Constants.QueryComponents.INTERSECT));
                    break;
                case CompoundType.Except:
                    _components.Add(new StringComponent(Constants.QueryComponents.EXCEPT));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        [UsedImplicitly]
        public Select Select()
        {
            return new Select(_components);
        }
    }
}
