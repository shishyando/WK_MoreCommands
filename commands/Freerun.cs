
using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

// freerun = godmode + deathgoo-stop + fullbright + infinitestamina
public static class FreerunCommand
{
    public static string[] Aliases => ["freerun"];
    public static CommandTag Tag => CommandTag.Player;
    public static bool Enabled;

    public static void ApplyFreerunState(bool enabled)
    {
        ENT_Player player = ENT_Player.playerObject;
        player?.SetGodMode(enabled);
        player?.InfiniteStaminaCommand(CommandHelpers.WhenEnabled(enabled));
        FXManager.Fullbright(CommandHelpers.WhenEnabled(enabled));
        DEN_DeathFloor deathgoo = DEN_DeathFloor.instance;
        deathgoo?.DeathGooToggle(CommandHelpers.WhenDisabled(enabled));
    }

    public static Action<string[]> GetCallback()
    {
        return args =>
        {
            CommandHelpers.UpdateEnabled(ref Enabled, args);
            if (Enabled)
            {
                Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabld();
            }
            ApplyFreerunState(Enabled);
        };
    }
}
