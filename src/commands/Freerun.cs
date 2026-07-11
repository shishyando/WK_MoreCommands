using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class FreerunCommand : TogglableCommandBase
{
    public override string[] Aliases => ["freerun"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Toggle god mode, paused deathgoo, fullbright, infinite stamina, and no-target";
    public override bool EnablesCheatsOnUse => true;

    public override void Execute(string[] args)
    {
        ENT_Player player = ENT_Player.playerObject;
        player?.SetGodMode(Enabled);
        player?.InfiniteStaminaCommand(WhenEnabled(Enabled));
        FXManager.Fullbright(WhenEnabled(Enabled));
        DEN_DeathFloor deathgoo = DEN_DeathFloor.instance;
        deathgoo?.DeathGooToggle(WhenDisabled(Enabled));
        CL_GameManager.gMan?.NoTarget(WhenEnabled(Enabled));
    }
}
