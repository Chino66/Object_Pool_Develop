using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;

public class AutoRecyclePoolExample : MonoBehaviour
{
    private void OnGUI()
    {
        OnPoolDatabaseGUI();
        OnPoolFactoryGUI();
        OnPoolFactoryGenericGUI();
    }

    private void OnPoolFactoryGenericGUI()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("OnPoolFactoryGenericGUI");
        if (GUILayout.Button("Get item"))
        {
            // 泛型池获取实例
            var item = PoolFactoryGenericItem.Pool.Get();
            item.Name = $"item{item.Id}";
            item.Id++;
            Debug.Log($"{item.ToString()}");

            // 使用完后可以不用主动回收,当GC释放实例时,将自动回收,缺点是时间点不可控
            // 也可以手动归还实例到池子
            PoolFactoryGenericItem.Pool.Return(item);
        }

        if (GUILayout.Button("Information"))
        {
            Debug.Log($"{PoolFactoryGenericItem.Pool.Information()}");
        }

        if (GUILayout.Button("GC"))
        {
            GC.Collect();
        }

        GUILayout.EndVertical();
    }

    private void OnPoolFactoryGUI()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("OnPoolFactoryGUI");
        if (GUILayout.Button("Get item"))
        {
            // 通常不会单独使用PoolFactory对象池,而是使用PoolFactory<T>
            // PoolFactory主要配合PoolDatabase使用,实现统一的对象池实例管理

            // PoolFactory继承自PoolFactory<T>且声明泛型T为IPoolable(可池化的)
            // 所以使用PoolFactory获取的实例需要实现IPoolable接口
            // 且使用PoolFactory获取的实例需要类型转换
            // 所以对于单一类型使用对象池,推荐使用PoolFactory<T>

            // Simple way
            var item = PoolFactoryItem.Pool.Get<PoolFactoryItem>();
            item.Name = $"item{item.Id}";
            item.Id++;
            Debug.Log($"{item.ToString()}");

            // Other way
            var item2 = (PoolFactoryItem) PoolFactoryItem.Pool.Get();
            item2.Name = $"item{item2.Id}";
            item2.Id++;
            Debug.Log($"{item2.ToString()}");

            // 使用完后可以不用主动回收,当GC释放实例时,将自动回收,缺点是时间点不可控
        }

        if (GUILayout.Button("Information"))
        {
            Debug.Log($"{PoolFactoryItem.Pool.Information()}");
        }

        if (GUILayout.Button("GC"))
        {
            GC.Collect();
        }

        GUILayout.EndVertical();
    }

    private void OnPoolDatabaseGUI()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("OnPoolDatabaseGUI");
        if (GUILayout.Button("Get item"))
        {
            // 使用PoolDatabase获取对象实例
            // 内部实现是通过泛型类型和PoolFactory的映射,实现实例的获取
            // 且可以自动回收和手动回收

            var item = PoolDatabase.Get<PoolDatabaseItem>();
            item.Name = $"item{item.Id}";
            item.Id++;
            Debug.Log($"{item.ToString()}");
            
            PoolDatabase.Return(item);

            // 使用完后可以不用主动回收,当GC释放实例时,将自动回收,缺点是时间点不可控
        }

        if (GUILayout.Button("Information"))
        {
            // Simple way
            Debug.Log($"{PoolDatabase.Information<PoolDatabaseItem>()}");

            // Other way
            Debug.Log($"{PoolDatabase.GetPoolFactory<PoolDatabaseItem>().Information()}");
        }

        if (GUILayout.Button("GC"))
        {
            GC.Collect();
        }

        GUILayout.EndVertical();
    }
}