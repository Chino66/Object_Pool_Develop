using System;
using ObjectPool;

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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PoolableItem()
        {
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