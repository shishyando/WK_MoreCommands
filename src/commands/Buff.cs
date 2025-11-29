using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class BuffCommand : TogglableCommandBase
{
    public override string[] Aliases => ["buff"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "buff everything, without perks";
    public override bool CheatsOnly => true;

    private readonly string BuffId = $"{MyPluginInfo.PLUGIN_GUID}.buffCommand";

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            ENT_Player player = ENT_Player.playerObject;
            if (player == null) return;
            if (Enabled)
            {
                BuffContainer grabBuffC = new()
                {
                    id = BuffId,
                    buffs = [
                        new() { id = "addReach", amount = 1.25f, maxAmount = 1.25f },
                        new() { id = "buffTimeMult", amount = 3f, maxAmount = 3f },
                        new() { id = "addExtraJumps", amount = 100, maxAmount = 100 },
                        new() { id = "addJumpBoost", amount = 0.6f, maxAmount = 0.6f },
                        new() { id = "grabAnything", amount = 1e9f, maxAmount = 1e9f },
                        new() { id = "addSpeed", amount = 3.5f, maxAmount = 3.5f },
                        new() { id = "addClimb", amount = 3.5f, maxAmount = 3.5f },
                        new() { id = "addJump", amount = 1.5f, maxAmount = 1.5f },
                        new() { id = "addStrike", amount = 100f, maxAmount = 100f },
                        new() { id = "addHammer", amount = 100f, maxAmount = 100f },
                        new() { id = "addCapacity", amount = 1000f, maxAmount = 1000f },
                    ],
                    loseOverTime = false,
                    multiplier = 1f, // buff amount should be defined by the buff itself, not the container
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
