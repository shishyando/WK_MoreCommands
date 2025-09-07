
using System;

namespace MoreCommands.Commands;

// freerun = noclip + godmode + deathgoo-stop + fullbright + infinitestamina
public sealed class FreerunCommand : Command<FreerunCommand>
{
    public override string Cmd => "freerun";
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
            player?.InfiniteStaminaCommand(WhenEnabled());
            FXManager.Fullbright(WhenEnabled());
            DEN_DeathFloor deathgoo = DEN_DeathFloor.instance;
            deathgoo?.DeathGooToggle(WhenDisabled());
        };
    }
}
