using HarmonyLib;
using MoreCommands.Common;
using UnityEngine;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(ENT_Player), "CreateCommands")]
public static class ENT_Player_CreateCommands_Patcher
{
    public static void Postfix(ENT_Player __instance)
    {
        foreach (var c in CommandRegistry.GetCommandsByTag(CommandTag.Player))
        {
            foreach (var alias in c.Aliases)
            {
                CommandConsole.AddCommand(alias, c.GetCallback(), false);
            }
        }
    }
}

[HarmonyPatch(typeof(ENT_Player), "Movement")]
public static class ENT_Player_Movement_Patcher
{
    public static readonly AccessTools.FieldRef<ENT_Player, CL_GameManager> gManRef = AccessTools.FieldRefAccess<ENT_Player, CL_GameManager>("gMan");
    public static readonly AccessTools.FieldRef<ENT_Player, Vector3> moveAxisRef = AccessTools.FieldRefAccess<ENT_Player, Vector3>("moveAxis");
    public static readonly AccessTools.FieldRef<ENT_Player, Vector3> velRef = AccessTools.FieldRefAccess<ENT_Player, Vector3>("vel");

    public static float NoclipSpeedMultiplier = 1f;

    public static void Postfix(ENT_Player __instance)
    {
        if (__instance.noclip && !gManRef(__instance).freecam && !gManRef(__instance).lockPlayerInput)
        {
            __instance.transform.position += (moveAxisRef(__instance) * 6f + velRef(__instance)) * Time.fixedDeltaTime * (NoclipSpeedMultiplier - 1f);
        }
    }
}


