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
        WKAssetDatabase assetDatabase = CL_AssetManager.GetFullCombinedAssetDatabase();

        IEnumerable<GameObject> joinedCollections = assetDatabase.entityPrefabs
                                            .Concat(assetDatabase.denizenPrefabs)
                                            .Concat(assetDatabase.itemPrefabs)
                                            .Where(x => x != null && x.name != null)
                                            .Distinct();
        Dictionary<string, GameEntity> entities = [];
        foreach (GameObject obj in joinedCollections)
        {
            GameEntity gameEntity = obj.GetComponentInChildren<GameEntity>(true);
            if (gameEntity == null || string.IsNullOrWhiteSpace(gameEntity.entityPrefabID)) continue;

            entities.TryAdd(gameEntity.entityPrefabID.ToLower(), gameEntity);
        }
        __instance.gameEntityPrefabs = [.. entities.Values];
    }
}

[HarmonyPatch(typeof(CL_GameManager), "Start")]
public static class CL_GameManager_Start_CommandRegistration_Patcher
{
    [HarmonyPostfix]
    public static void RegisterWorldCommands()
    {
        CommandRegistration.AddCommandsByTag(CommandTag.World);
    }
}
