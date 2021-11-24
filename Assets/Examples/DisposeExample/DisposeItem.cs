using System;
using System.Collections.Generic;
using UnityEngine;

public static class DisposablePool
{
    private static Queue<DisposeItem> _queue;

    static DisposablePool()
    {
        _queue = new Queue<DisposeItem>(1000);
    }

    public static DisposeItem Get()
    {
        if (_queue.Count <= 0)
        {
            return new DisposeItem();
        }

        // Debug.Log("Get");
        return _queue.Dequeue();
    }

    public static void Return(DisposeItem item)
    {
        _queue.Enqueue(item);
        // Debug.Log("Return");
    }
}

public class DisposeItem : IDisposable
{
    public string Name = "_";
    public string Level;
    public string S;

    public DisposeItem()
    {
    }

    public void SetName(string name)
    {
        // Debug.Log($"SetName {Name}");
        Name = name;
    }

    bool _disposed;

    public void Dispose()
    {
        // Debug.Log($"{Name} Dispose");
        Dispose(true);
        GC.SuppressFinalize(this); //标记gc不在调用析构函数
    }

    ~DisposeItem()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        // Debug.Log($"{Name} Dispose {disposing}");

        // tip 关于在析构函数回收对象的探讨
        // https://stackoverflow.com/questions/14090765/when-to-return-an-object-back-to-its-pool
        DisposablePool.Return(this);

        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
        }

        _disposed = true;
    }
}