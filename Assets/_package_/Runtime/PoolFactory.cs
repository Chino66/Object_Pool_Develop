using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class PoolFactory<T> where T : new()
    {
        private Queue<T> _queue;

        public bool AllowRecycle = true;

        public int MaxSize = 512;

        public int InitializeSize = 512 / 8;

        public int Count => _queue.Count;

        public PoolFactory()
        {
            _queue = new Queue<T>(InitializeSize);
        }


        public T Get()
        {
            if (_queue.Count > 0)
            {
                Debug.Log("Get");
                var item = _queue.Dequeue();
                return item;
            }

            Debug.Log("Constructor");
            return new T();
        }

        public bool Return(T t)
        {
            if (AllowRecycle == false)
            {
                return false;
            }

            if (_queue.Count >= MaxSize)
            {
                Debug.Log($"When return {typeof(T).Name} instance the queue is full, MaxSize is {MaxSize}");
                return false;
            }

            Debug.Log("Return");
            _queue.Enqueue(t);

            return true;
        }
    }
}