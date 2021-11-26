using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class PoolFactory<T>
    {
        protected Queue<T> _queue;

        public bool AllowRecycle = true;

        public int MaxSize = 512;

        public int InitializeSize => MaxSize / 8;

        public int Count => _queue.Count;

        public int GetCount { get; protected set; }
        public int ReuseCount { get; protected set; }
        public int CreateCount { get; protected set; }
        public int ReturnCount { get; protected set; }

        public int AutoRecycleCount { get; protected set; }
        public int RecycleCount { get; protected set; }

        public PoolFactory()
        {
            Initialize(MaxSize);
        }

        public PoolFactory(int capacity)
        {
            Initialize(capacity);
        }

        private void Initialize(int capacity)
        {
            MaxSize = capacity;
            _queue = new Queue<T>(InitializeSize);
            GetCount = CreateCount = ReturnCount = 0;
        }

        public T Get()
        {
            GetCount++;
            if (_queue.Count > 0)
            {
                ReuseCount++;
                return _queue.Dequeue();
            }

            CreateCount++;
            return (T) Activator.CreateInstance(typeof(T));
        }

        public bool Return(T t)
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

        public bool AutoRecycle(T t)
        {
            AutoRecycleCount++;
            var rst = Return(t);

            if (rst)
            {
                GC.ReRegisterForFinalize(t);
            }

            return rst;
        }
        
        public string Information()
        {
            return
                $"Count is {Count}, GetCount is {GetCount}, ReuseCount is {ReuseCount}, CreateCount is {CreateCount}, ReturnCount is {ReturnCount}, ";
        }
    }
}