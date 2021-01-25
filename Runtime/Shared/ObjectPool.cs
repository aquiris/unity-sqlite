using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;

namespace Aquiris.SQLite.Shared
{
    public class ObjectPool<T> {
        private readonly ConcurrentBag<T> bag;
        private readonly Func<T> factory;

        public ObjectPool(Func<T> factory) {
            bag = new ConcurrentBag<T>();
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.Pay(this.Rent());
        }

        [UsedImplicitly]
        public T Rent()
        {
            return bag.TryTake(out T item) ? item : factory();
        }

        [UsedImplicitly]
        public void Pay(T item) {
            bag.Add(item);
        }
    }
}
