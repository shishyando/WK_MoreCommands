using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class FlashCommand : TogglableCommandBase
{
    public override string[] Aliases => ["flash"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "freerun + buff";
    public override bool CheatsOnly => true;

    public override Action<string[]> GetLogicCallback()
    {
        return CommandRegistry.GetCallback<FreerunCommand>(withSuffix: false)
            + CommandRegistry.GetCallback<BuffCommand>(withSuffix: false);
    }
}
