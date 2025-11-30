using System;
using System.Collections.Generic;

namespace MoreCommands.HandleProviders;

public class Items : HandleProvider<Item>
{
    public override Func<Item, string> Name {get;} = x => x?.prefabName?.ToLower() ?? null;
    public override Func<Item, Item> Finalizer {get;} = obj => obj.GetClone();
    private List<Item> _items = [];
    
    public void Initialize(List<Item> items)
    {
        _items = items;
    }

    public void InitializeFromAssetDatabase()
    {
        var itemPrefabs = CL_AssetManager.GetFullCombinedAssetDatabase().itemPrefabs;
        foreach (UnityEngine.GameObject prefab in itemPrefabs)
        {
            Item_Object component = prefab.GetComponent<Item_Object>();
            if (component != null)
            {
                _items.Add(component.itemData);
            }
        }
    }

    public override Handle<Item> Handle()
    {
        if (_items.Count == 0) InitializeFromAssetDatabase();
        return new(_items, Name, Finalizer);
    }
}
