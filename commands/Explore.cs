
using System;

namespace MoreCommands.Commands;

// explore = noclip + godmode + deathgoo-stop + fullbright + infinitestamina
public sealed class ExploreCommand : Command<ExploreCommand>
{
    public override string Cmd => "explore";
    public override CommandTag Tag => CommandTag.Player;

    public override Action<string[]> GetCallback()
    {
        return args =>
        {
            UpdateEnabled(args);
            if (Enabled)
            {
                Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabld();
            }
            ENT_Player player = ENT_Player.playerObject;
            player?.SetGodMode(Enabled);
            player?.Noclip(WhenEnabled());
            player?.InfiniteStaminaCommand(WhenEnabled());
            FXManager.Fullbright(WhenEnabled());
            DEN_DeathFloor deathgoo = DEN_DeathFloor.instance;
            deathgoo?.DeathGooToggle(WhenDisabled());
        };
    }
}
