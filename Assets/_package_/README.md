# ObjectPool

项目地址：[https://github.com/Chino66/Object\_Pool\_Develop](https://github.com/Chino66/Object_Pool_Develop)

基础对象池库，提供了一个简单对象池使用

## 基础使用

基础对象池使用

    public Pool<CommonPoolItem> Pool;
    // 构造对象池并初始化
    Pool = new Pool<CommonPoolItem>();
    Pool.SetCreateFunc(() => new CommonPoolItem());
    Pool.SetGetAction((item) => { item.Id++; });
    // 获取对象
    var item = Pool.Get();
    // 归还对象
    Pool.Return(item);

## 可自动回收的对象池

基于析构函数的对象池，可以实现对象被GC时检测泄漏和自动回收

### 使用

代码示例：

    public class PoolDatabaseItem : IPoolable{
        public int Id;
        #region auto recycle
        /// <summary>
        /// 实现对象自动回收的关键就是在析构时将实例归还给池子
        /// 同时需要设置GC.ReRegisterForFinalize(this)告知GC这个实例在下一次释放时还要调用析构函数（这个设置在池子内部实现）
        /// </summary>
        ~PoolDatabaseItem(){
            PoolDatabase.AutoRecycle(this);
        }
        #endregion
    }

可自动回收对象需要实现IPoolable接口同时实现析构函数：

    ~PoolDatabaseItem(){
      PoolDatabase.AutoRecycle(this);
    }

    // 获取对象
    var item = PoolDatabase.Get<PoolDatabaseItem>();
    // 归还对象，如果不归还，则在下一次GC的时候会自动归还，可以在这里添加一些警告告知开发者这里造成了泄漏
    PoolDatabase.Return(item);

:::
如果获取对象使用完毕后不归还，则在下一次GC的时候会自动归还，可以在这里添加一些警告告知开发者这里造成了泄漏
:::

可以通过Information方法获取当前对象池信息。

    PoolDatabase.Information<PoolDatabaseItem>()