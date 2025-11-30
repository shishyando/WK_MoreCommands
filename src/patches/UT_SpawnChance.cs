using System.Collections.Generic;
using HarmonyLib;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(UT_SpawnChance), "Start")]
public static class UT_SpawnChance_Start_Patcher
{
    public static bool AlwaysSpawn = false;

    public static HashSet<string> ExcludeFromAlwaysSpawn = [
        "Level_Secret_Entrance_Blocked",
        "Secret area spawn",
    ];

    public static bool Prefix(UT_SpawnChance __instance)
    {
        if (AlwaysSpawn) {
            __instance.gameObject.SetActive(!ExcludeFromAlwaysSpawn.Contains(__instance.name));
            return false;
        }
        return true;
    }
}


