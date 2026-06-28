using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class FlashCommand : TogglableCommandBase
{
    public override string[] Aliases => ["flash"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "freerun + buff";
    public override bool EnablesCheatsOnUse => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            CommandRegistry.SetTogglable<FreerunCommand>(Enabled);
            CommandRegistry.SetTogglable<BuffCommand>(Enabled);
        };
    }
}
