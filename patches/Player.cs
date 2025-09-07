using System;
using HarmonyLib;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(ENT_Player), "CreateCommands")]
public static class ENT_Player_Patcher
{
    public static bool active = false;
    [HarmonyPostfix]
    public static void AddMorePlayerCommands(ENT_Player __instance)
    {

        // explore = noclip + godmode + deathgoo-stop + fullbright + infiniteStamina
        CommandConsole.AddCommand("explore", new Action<string[]>((string[] args) =>
        {
            if (args.Length == 0)
            {
                active = !active;
            }
            else if (!bool.TryParse(args[0], out active))
            {
                MoreCommandsPlugin.Logger.LogInfo($"Unable to parse `{args}`, arg needs to be a boolean (true/false/0/1).");
                return;
            }
            UpdateExploreCommand(__instance);
        }), false);
    }

    public static void UpdateExploreCommand(ENT_Player __instance)
    {
        string[] activeArgs = { active.ToString().ToLowerInvariant() };
        if (active)
        {
            Accessors.CommandConsoleAccessor.EnableCheats();
        }
        __instance.SetGodMode(active);
        __instance.Noclip(activeArgs);
        __instance.InfiniteStaminaCommand(activeArgs);
        FXManager.Fullbright(activeArgs);
        DEN_DeathFloor deathgoo = DEN_DeathFloor.instance;
        if (deathgoo != null) {
            deathgoo.DeathGooToggle(activeArgs);
        }
    }

}
