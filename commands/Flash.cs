
using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

// flash = freerun + speedy
public sealed class FlashCommand : OneshotCommand<FlashCommand>
{
    public override string[] Aliases => ["flash"];
    public override CommandTag Tag => CommandTag.Player;

    public override Action<string[]> GetCallback()
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
