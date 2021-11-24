using System;
using System.Collections;
using System.Collections.Generic;
using Examples.DisposeExample;
using UnityEngine;

public class DisposeExample : MonoBehaviour
{
    private const int count = 10000;
    private List<DisposeItem> _list;
    private List<PoolableItem> _poolableItems;

    void Start()
    {
        _list = new List<DisposeItem>(count);
        _poolableItems = new List<PoolableItem>(count);
    }

    private void OnGUI()
    {
        DisposePoolGUI();

        // if (GUILayout.Button("GC Test"))
        // {
        //     GCTest();
        // }
        //
        // if (GUILayout.Button("null"))
        // {
        //     _list = null;
        // }
        //
        // if (GUILayout.Button("clear"))
        // {
        //     _list?.Clear();
        // }

        // if (GUILayout.Button("Get DisposeItem"))
        // {
        //     var item = DisposablePool.Get();
        //     Debug.Log($"name is {item.Name}");
        // }
    }

    private void DisposePoolGUI()
    {
        if (GUILayout.Button("get PoolableItem"))
        {
            var item = PoolableItem.Pool.Get();
            item.Name = "001";
            Debug.Log($"item name is {item.Name}");
        }

        if (GUILayout.Button("get PoolableItem2"))
        {
            var item = PoolableItem.Pool.Get();
            Debug.Log($"item name is {item.Name}");
        }

        if (GUILayout.Button("GC"))
        {
            GC.Collect();
        }

        if (GUILayout.Button("Count"))
        {
            Debug.Log($"{PoolableItem.Pool.Count}");
        }
    }

    private void GCTest()
    {
        _list.Clear();
        for (int i = 0; i < count; i++)
        {
            var item = DisposablePool.Get();
            item.SetName($"ssssss");
            // Debug.Log($"name is {item.Name}");
            _list.Add(item);
        }
    }
}