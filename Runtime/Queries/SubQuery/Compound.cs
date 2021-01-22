using System;
using Aquiris.SQLite.Queries.Components;
using Aquiris.SQLite.Shared;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Queries
{
    public enum CompoundType
    {
        union,
        unionAll,
        intersects,
        except,
    }
    
    public readonly struct Compound
    {
        private readonly QueryComponents _components;

        internal Compound(QueryComponents components, CompoundType type)
        {
            _components = components;
            switch (type)
            {
                case CompoundType.union:
                    _components.Add(new StringComponent(Constants.QueryComponents.UNION));
                    break;
                case CompoundType.unionAll:
                    _components.Add(new StringComponent(Constants.QueryComponents.UNION));
                    _components.Add(new StringComponent(Constants.QueryComponents.SELECT_ALL));
                    break;
                case CompoundType.intersects:
                    _components.Add(new StringComponent(Constants.QueryComponents.INTERSECT));
                    break;
                case CompoundType.except:
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