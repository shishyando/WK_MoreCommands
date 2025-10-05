using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class FlashCommand : CommandBase
{
    public override string[] Aliases => ["flash"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "freerun + buff";
    public override bool CheatsOnly => true;

    protected override Action<string[]> GetLogicCallback()
    {
        return CommandRegistry.GetCallback<FreerunCommand>()
            + CommandRegistry.GetCallback<BuffCommand>();
    }
}
