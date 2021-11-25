using System;
using System.Collections.Generic;

namespace ObjectPool
{
    public static class PoolDatabase
    {
        private static readonly Dictionary<Type, PoolFactory> _pools;

        static PoolDatabase()
        {
            _pools = new Dictionary<Type, PoolFactory>();
        }

        public static PoolFactory GetPoolFactory<T>() where T : IPoolable
        {
            var type = typeof(T);

            if (!_pools.TryGetValue(type, out var poolFactory))
            {
                poolFactory = new PoolFactory();
                _pools.Add(type, poolFactory);
            }

            return poolFactory;
        }

        public static T Get<T>() where T : IPoolable
        {
            var type = typeof(T);

            if (!_pools.TryGetValue(type, out var poolFactory))
            {
                poolFactory = new PoolFactory();
                _pools.Add(type, poolFactory);
            }

            return (T) poolFactory.Get<T>();
        }

        public static bool Return<T>(T t) where T : IPoolable
        {
            var type = typeof(T);

            if (!_pools.TryGetValue(type, out var poolFactory))
            {
                poolFactory = new PoolFactory();
                _pools.Add(type, poolFactory);
            }

            return poolFactory.Return(t);
        }

        public static bool AutoRecycle<T>(T t) where T : IPoolable
        {
            var rst = Return(t);

            if (rst)
            {
                GC.ReRegisterForFinalize(t);
            }

            return rst;
        }

        public static string Information<T>() where T : IPoolable
        {
            var type = typeof(T);

            if (_pools.TryGetValue(type, out var poolFactory))
            {
                return poolFactory.Information();
            }

            return $"{type.FullName} was not pool";
        }
    }
}