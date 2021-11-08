using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class Pool<T>
    {
        protected readonly Queue<T> _queue;

        protected Func<T> CreateFunc;

        protected Action<T> GetAction;

        protected Action<T> ReturnAction;

        public Pool()
        {
            _queue = new Queue<T>();
        }

        public Pool<T> SetCreateFunc(Func<T> func)
        {
            CreateFunc = func;
            return this;
        }

        public Pool<T> SetGetAction(Action<T> action)
        {
            GetAction = action;
            return this;
        }

        public Pool<T> SetReturnAction(Action<T> action)
        {
            ReturnAction = action;
            return this;
        }

        protected virtual T Create()
        {
            return CreateFunc.Invoke();
        }

        public virtual T Get()
        {
            var item = _queue.Count > 0 ? _queue.Dequeue() : Create();

            GetAction?.Invoke(item);

            return item;
        }

        public virtual void Return(T item, bool checkContains = false)
        {
            if (item == null)
            {
                return;
            }

            if (checkContains && _queue.Contains(item))
            {
                return;
            }

            ReturnAction?.Invoke(item);

            _queue.Enqueue(item);
        }
    }
}