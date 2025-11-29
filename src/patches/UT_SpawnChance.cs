using System.Collections.Generic;
using HarmonyLib;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(UT_SpawnChance), "Start")]
public static class UT_SpawnChance_Start_Patcher
{
    public static bool AlwaysSpawn = false;

    public static HashSet<string> ExcludeFromAlwaySpawn = [
        "Level_Secret_Entrance_Blocked",
        "Secret area spawn",
    ];

    public static bool Prefix()
    {

        return !AlwaysSpawn;
    }
}


