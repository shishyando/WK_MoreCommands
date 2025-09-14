using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ExploreCommand : TogglableCommandBase
{
    public override string[] Aliases => ["explore"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "freerun + noclip";
    public override bool CheatsOnly => true;

    protected override Action<string[]> GetLogicCallback()
    {
        return CommandRegistry.GetCallback<FreerunCommand>()
            + new Action<string[]>(args =>
            {
                ENT_Player player = ENT_Player.playerObject;
                player?.Noclip(WhenEnabled(Enabled));
            });
    }
}
