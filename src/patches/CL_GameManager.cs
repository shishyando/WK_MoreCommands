using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MoreCommands.Common;
using UnityEngine;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(CL_GameManager), "Awake")]
public static class CL_GameManager_Awake_Patcher
{
    [HarmonyPostfix]
    public static void PatchDatabases(CL_GameManager __instance)
    {
        WKAssetDatabase baseAssetDatabase = CL_AssetManager.GetBaseAssetDatabase();

        IEnumerable<GameObject> joinedCollections = baseAssetDatabase.entityPrefabs
                                            .Concat(baseAssetDatabase.denizenPrefabs)
                                            .Concat(baseAssetDatabase.itemPrefabs)
                                            .Where(x => x != null && x.name != null)
                                            .Distinct();
        Dictionary<string, Item> items = [];
        Dictionary<string, GameEntity> entities = [];
        foreach (GameObject obj in joinedCollections)
        {
            GameEntity gameEntity = obj.GetComponentInChildren<GameEntity>() ?? obj.AddComponent<GameEntity>();
            entities.TryAdd(obj.name.ToLower(), gameEntity);
            
            Item_Object itemObject = obj.GetComponentInChildren<Item_Object>();
            
            if (itemObject != null) {
                items.TryAdd(itemObject.itemData.prefabName, itemObject.itemData);
            }
        }
        __instance.gameEntityPrefabs = [.. entities.Values];
        PrefabsItems.Initialize(items.Values.ToList());
    }
}
