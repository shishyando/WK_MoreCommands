using System;
using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(ENT_Player), "CreateCommands")]
public static class ENT_Player_Patcher
{
    public static bool exploreActive = false;
    [HarmonyPostfix]
    public static void AddMorePlayerCommands(ENT_Player __instance)
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
