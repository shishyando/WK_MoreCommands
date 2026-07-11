using System.Collections.Generic;
using HarmonyLib;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(UT_SpawnChance), "Start")]
public static class UT_SpawnChance_Start_Patcher
{
    public static bool AlwaysSpawn;

    public static readonly HashSet<string> ExcludeFromAlwaysSpawn = [
        "Level_Secret_Entrance_Blocked",
        "Secret area spawn",
    ];

    public static bool Prefix(UT_SpawnChance __instance)
    {
        if (!AlwaysSpawn || !__instance.checkOnStart) return true;

        bool shouldSpawn = !ExcludeFromAlwaysSpawn.Contains(__instance.name);
        __instance.gameObject.SetActive(shouldSpawn);
        if (shouldSpawn) __instance.onSuccess.Invoke();
        else __instance.onFail.Invoke();
        return false;
    }
}
