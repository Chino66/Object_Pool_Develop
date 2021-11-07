using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class Pool<T>
    {
        private readonly Queue<T> _queue;

        private Func<T> _createFunc;

        private Action<T> _getAction;

        private Action<T> _returnAction;

        public Pool()
        {
            _queue = new Queue<T>();
        }

        public Pool<T> SetCreateFunc(Func<T> func)
        {
            _createFunc = func;
            return this;
        }

        public Pool<T> SetGetAction(Action<T> action)
        {
            _getAction = action;
            return this;
        }

        public Pool<T> SetReturnAction(Action<T> action)
        {
            _returnAction = action;
            return this;
        }

        private T Create()
        {
            return _createFunc.Invoke();
        }

        public T Get()
        {
            T item;
            if (_queue.Count > 0)
            {
                item = _queue.Dequeue();
            }

            item = Create();

            _getAction?.Invoke(item);

            return item;
        }

        public void Return(T item)
        {
            if (item == null)
            {
                return;
            }

            if (_queue.Contains(item))
            {
                return;
            }

            _returnAction?.Invoke(item);

            _queue.Enqueue(item);
        }
    }
}