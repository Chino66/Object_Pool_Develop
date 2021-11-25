using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class PoolFactory : PoolFactory<IPoolable>
    {
        public T Get<T>() where T : IPoolable
        {
            GetCount++;
            if (_queue.Count > 0)
            {
                ReuseCount++;
                return (T) _queue.Dequeue();
            }

            CreateCount++;
            return (T) Activator.CreateInstance(typeof(T));
        }

        public bool Return<T>(T t) where T : IPoolable
        {
            if (AllowRecycle == false)
            {
                Debug.Log($"When return {typeof(T).Name} instance, AllowRecycle is {MaxSize}");
                return false;
            }

            if (_queue.Count >= MaxSize)
            {
                Debug.Log($"When return {typeof(T).Name} instance the queue is full, MaxSize is {MaxSize}");
                return false;
            }

            ReturnCount++;
            _queue.Enqueue(t);

            return true;
        }

        public bool AutoRecycle<T>(T t) where T : IPoolable
        {
            var rst = Return(t);

            if (rst)
            {
                GC.ReRegisterForFinalize(t);
            }

            return rst;
        }
    }
}