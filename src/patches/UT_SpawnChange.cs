using HarmonyLib;
using UnityEngine;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(UT_SpawnChance), "Start")]
public static class UT_SpawnChance_Start_Patcher
{
    public static bool AlwaysSpawn = false;

    public static bool Prefix()
    {
        return !AlwaysSpawn;
    }
}


