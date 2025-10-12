
using System.Collections.Generic;
using System.Linq;

namespace MoreCommands.Common;

public static class ItemGod
{
    public static List<Item> GetAllItems()
    {
        var itemPrefabs = CL_AssetManager.GetFullCombinedAssetDatabase().itemPrefabs;
        List<Item> items = [];
        foreach (UnityEngine.GameObject prefab in itemPrefabs)
        {
            Item_Object component = prefab.GetComponent<Item_Object>();
            if (component != null)
            {
                items.Add(component.itemData);
            }
        }
        return items;
    }

    public static Item GetItemByPrefabName(string prefabName)
    {
        return GetAllItems().Find(x => x.prefabName.ToLower() == prefabName);
    }

    public static Item FindAndClone(string prefabSubstr)
    {
        var item = GetAllItems().Find(x => x.prefabName.ToLower().Contains(prefabSubstr));
        if (item == null) return null;
        return item.GetClone();
    }

    public static List<string> PrefabNames()
    {
        List<Item> items = GetAllItems();
        return [.. items.Select(x => x.prefabName.ToLower())];
    }

    public static string PrefabNames(string delimiter)
    {
        return string.Join(delimiter, PrefabNames());
    }
}
