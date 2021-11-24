using System;
using ObjectPool;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace Examples.DisposeExample
{
    public class PoolableItem : IPoolable
    {
        public static PoolFactory<PoolableItem> Pool;

        static PoolableItem()
        {
            Pool = new PoolFactory<PoolableItem>();
        }

        public string Name;
        public int Index;

        public PoolableItem()
        {
        }

        #region dispose

        bool _disposed;

        public void Dispose()
        {
//            Debug.Log("public void Dispose()");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PoolableItem()
        {
//            Debug.Log("~PoolableItem()");
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            var rst = Pool.Return(this);

            if (rst)
            {
                GC.ReRegisterForFinalize(this);
                return;
            }

            _disposed = true;
            if (disposing)
            {
                // todo dispose something
            }
        }

        #endregion
    }
}