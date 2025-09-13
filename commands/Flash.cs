
using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

// flash = freerun + speedy
public static class FlashCommand
{
    public static string[] Aliases => ["flash"];
    public static CommandTag Tag => CommandTag.Player;

    public static Action<string[]> GetCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabld();
            ENT_Player player = ENT_Player.playerObject;
            if (player == null) return;
            player?.SetGodMode(true);
            player?.InfiniteStaminaCommand(["true"]);
            FXManager.Fullbright(["true"]);
            DEN_DeathFloor deathgoo = DEN_DeathFloor.instance;
            deathgoo?.DeathGooToggle(["false"]);

        };
    }
}
