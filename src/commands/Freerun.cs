using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class FreerunCommand : TogglableCommandBase
{
    public override string[] Aliases => ["freerun"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "godmode + deathgoo-stop + fullbright + infinitestamina + notarget";
    public override bool CheatsOnly => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            ENT_Player player = ENT_Player.playerObject;
            player?.SetGodMode(Enabled);
            player?.InfiniteStaminaCommand(WhenEnabled(Enabled));
            FXManager.Fullbright(WhenEnabled(Enabled));
            DEN_DeathFloor deathgoo = DEN_DeathFloor.instance;
            deathgoo?.DeathGooToggle(WhenDisabled(Enabled));
            CL_GameManager.gMan?.NoTarget(WhenEnabled(Enabled));
        };
    }
}
