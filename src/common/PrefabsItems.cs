using System.Collections.Generic;
using System.Linq;

namespace MoreCommands.Common;

public static class PrefabsItems
{
    private static List<Item> _items = [];
    
    public static void Initialize(List<Item> items)
    {
        _items = items;
    }

    public static void InitializeFromAssetDatabase()
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

    // Item
    // All
    public static List<Item> AllItems()
    {
        if (_items.Count == 0) InitializeFromAssetDatabase();
        return _items;
    }
    // Filtered
    public static List<Item> FilterItems(string filter)
    {
        return AllItems().FindAll(x => { return Helpers.Substr(x.prefabName, filter.ToLower()); });
    }
    // Any
    public static Item AnyItem(string filter)
    {
        return FilterItems(filter).FirstOrDefault();
    }
    // Any clone
    public static Item AnyItemClone(string filter)
    {
        return FilterItems(filter).FirstOrDefault()?.GetClone();
    }
 
    // Names
    // All
    public static List<string> AllItemNames()
    {
        return [.. AllItems().Select(x => {return x.prefabName.ToLower();})];
    }
    // All Joined
    public static string JoinAllItemNames(string delimiter = "\n- ")
    {
        return string.Join(delimiter, AllItemNames());
    }
    // Filtered
    public static List<string> FilteredItemNames(string filter)
    {
        return [.. FilterItems(filter).Select(x => {return x.prefabName.ToLower();})];
    }
    // Filtered Joined
    public static string JoinFilteredItemNames(string filter, string delimiter = "\n- ")
    {
        return string.Join(delimiter, FilteredItemNames(filter));
    }
    // Any
    public static string AnyItemName(string filter)
    {
        return AnyItem(filter)?.prefabName?.ToLower() ?? null;
    }


    // Command helpers
    public static Item ItemCloneForCommandFromArgs(string[] args)
    {
        if (args.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Available items:\n- {JoinAllItemNames()}");
            return null;
        }
        var clone = AnyItemClone(args[0]);
        if (clone == null)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"No such item: {args[0]}");
            return null;
        }
        Accessors.CommandConsoleAccessor.EchoToConsole($"Given item: {Colors.Highlighted(clone.prefabName.ToLower())}");
        return clone;
    }
}
