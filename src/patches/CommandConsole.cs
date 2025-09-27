using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(CommandConsole), "Awake")]
public static class CommandConsole_Patcher
{
    [HarmonyPostfix]
    public static void AddMorePlayerCommands(CommandConsole __instance)
    {
        foreach (var c in CommandRegistry.GetCommandsByTag(CommandTag.Console))
        {
            foreach (var alias in c.Aliases)
            {
                CommandConsole.AddCommand(alias, c.GetCallback(), false);
            }
        }
    }
}
