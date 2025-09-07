using System;
using HarmonyLib;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(ENT_Player), "CreateCommands")]
public static class ENT_Player_Patcher
{
    public static bool exploreActive = false;
    [HarmonyPostfix]
    public static void AddMorePlayerCommands(ENT_Player __instance)
    {

        // explore = noclip + godmode + deathgoo-stop + fullbright + infiniteStamina
        CommandConsole.AddCommand("explore", new Action<string[]>((string[] args) =>
        {
            if (args.Length == 0)
            {
                exploreActive = !exploreActive;
            }
            else if (!bool.TryParse(args[0], out exploreActive))
            {
                MoreCommandsPlugin.Logger.LogInfo($"Unable to parse `{args}`, arg needs to be a boolean (true/false/0/1).");
                return;
            }
            UpdateExploreCommand(__instance);
        }), false);
    }

    public static void UpdateExploreCommand(ENT_Player __instance)
    {
        string[] posArgs = { exploreActive.ToString().ToLowerInvariant() };
        string[] negArgs = { (!exploreActive).ToString().ToLowerInvariant() };
        if (exploreActive)
        {
            Accessors.CommandConsoleAccessor.EnableCheats();
        }
        __instance.SetGodMode(exploreActive);
        __instance.Noclip(posArgs);
        __instance.InfiniteStaminaCommand(posArgs);
        FXManager.Fullbright(posArgs);
        DEN_DeathFloor deathgoo = DEN_DeathFloor.instance;
        if (deathgoo != null) {
            deathgoo.DeathGooToggle(negArgs);
        }
    }

}
