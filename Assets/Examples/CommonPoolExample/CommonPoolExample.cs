using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;

/// <summary>
/// 常规对象池使用示例
/// </summary>
public class CommonPoolExample : MonoBehaviour
{
    public Pool<CommonPoolItem> Pool;

    void Start()
    {
        // 定义一个对象池
        Pool = new Pool<CommonPoolItem>();
        // 设置创建实例方法
        // 从池中Get对象实例时,如果池中没有实例,则进行创建实例方法
        // 如果没有设置创建实例方法,则默认构造实例
        Pool.SetCreateFunc(() => new CommonPoolItem());
        // 设置Get实例时的动作
        Pool.SetGetAction((item) => { item.Id++; });
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Get item"))
        {
            // 从对象池获取实例
            var item = Pool.Get();
            item.Name = $"Item{item.Id}";
            Debug.Log($"{item.ToString()}");
            // 使用后归还实例到池子
            Pool.Return(item);
        }

        if (GUILayout.Button("Pool Information"))
        {
            Debug.Log($"{Pool.Information()}");
        }
    }
}