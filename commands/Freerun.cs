
using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

// freerun = godmode + deathgoo-stop + fullbright + infinitestamina
public static class FreerunCommand
{
    public static string[] Aliases => ["freerun"];
    public static CommandTag Tag => CommandTag.Player;
    public static bool Enabled;

    public static Action<string[]> GetCallback()
    {
        return args =>
        {
            CommandHelpers.UpdateEnabled(ref Enabled, args);
            if (Enabled)
            {
                Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabld();
            }
            ENT_Player player = ENT_Player.playerObject;
            player?.SetGodMode(Enabled);
            player?.InfiniteStaminaCommand(CommandHelpers.WhenEnabled(Enabled));
            FXManager.Fullbright(CommandHelpers.WhenEnabled(Enabled));
            DEN_DeathFloor deathgoo = DEN_DeathFloor.instance;
            deathgoo?.DeathGooToggle(CommandHelpers.WhenDisabled(Enabled));
        };
    }
}
