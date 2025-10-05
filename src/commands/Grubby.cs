using System;
using System.Collections.Generic;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GrubbyCommand : TogglableCommandBase
{
    public override string[] Aliases => ["grubby"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "grab anything";
    public override bool CheatsOnly => true;

    private readonly string BuffId = $"{MyPluginInfo.PLUGIN_GUID}.grubbyCommand";

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabled();
            ENT_Player player = ENT_Player.playerObject;
            if (player == null) return;
            if (Enabled)
            {
                BuffContainer grabBuffC = new()
                {
                    id = BuffId,
                    buffs = [new() { id = "grabAnything", amount = 1e9f, maxAmount = 1e9f }],
                    loseOverTime = false
                };
                player.Buff(grabBuffC);
            }
            else
            {
                player.curBuffs.RemoveBuffContainer(BuffId);
            }
        };
    }
}
