using System;
using System.Linq;

namespace MoreCommands.HandleProviders;

public class Items : HandleProvider<Item>
{
    public override Func<Item, string> Name { get; } = item => item?.prefabName;

    public override Item FromCommand(string[] args)
    {
        return base.FromCommand(args)?.GetClone();
    }

    public override Handle<Item> Handle()
    {
        return new(
            CL_AssetManager.GetFullCombinedAssetDatabase().itemPrefabs
                .Select(prefab => prefab?.GetComponent<Item_Object>()?.itemData)
                .Where(item => item != null),
            Name);
    }
}
