using HarmonyLib;

namespace MoreCommands.Patches;


[HarmonyPatch(typeof(GameEntity), "Start")]
public static class GameEntity_Start_Patcher
{
    static void Postfix(GameEntity __instance)
    {
        Outlines.OutlinesController.RegisterEntity(__instance);
    }

}

[HarmonyPatch(typeof(GameEntity), "OnDestroy")]
public static class GameEntity_OnDestroy_Patcher
{
    static void Prefix(GameEntity __instance)
    {
        Outlines.OutlinesController.RegisterEntity(__instance);
    }

}
