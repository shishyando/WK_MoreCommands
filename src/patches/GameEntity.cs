using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using MoreCommands.Common;

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
        Outlines.OutlinesController.UnregisterEntity(__instance);
    }

}

[HarmonyPatch]
public static class AIGameEntity_Update_Patcher
{
    private static readonly string[] UpdateMethodNames =
        ["Update", "FixedUpdate", "LateUpdate", "TickUpdate", "SessionUpdate"];

    static IEnumerable<MethodBase> TargetMethods()
    {
        return AccessTools.GetTypesFromAssembly(typeof(AIGameEntity).Assembly)
            .Where(type => typeof(AIGameEntity).IsAssignableFrom(type))
            .SelectMany(type => UpdateMethodNames.Select(name => AccessTools.DeclaredMethod(type, name, Type.EmptyTypes)))
            .Where(method => method != null && !method.IsStatic)
            .Distinct();
    }

    static bool Prefix()
    {
        return !FreezeAiController.Frozen;
    }
}
