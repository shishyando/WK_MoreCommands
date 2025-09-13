
using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

// explore = noclip + godmode + deathgoo-stop + fullbright + infinitestamina
public static class ExploreCommand
{
    public static string[] Aliases => ["explore"];
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
            player?.Noclip(CommandHelpers.WhenEnabled(Enabled));
        };
    }
}
