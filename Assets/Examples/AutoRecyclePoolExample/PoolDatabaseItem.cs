using ObjectPool;

public class PoolDatabaseItem : IPoolable
{
    public string Name;
    public int Id;

    public override string ToString()
    {
        return $"Name is {Name}, Id is {Id}";
    }

    #region auto recycle

    /// <summary>
    /// 实现对象自动回收的关键就是在析构时将实例归还给池子
    /// 同时需要设置GC.ReRegisterForFinalize(this)告知GC这个实例在下一次释放时还要调用析构函数
    /// 这个设置在池子内部实现
    /// </summary>
    ~PoolDatabaseItem()
    {
        PoolDatabase.AutoRecycle(this);
    }

    #endregion
}