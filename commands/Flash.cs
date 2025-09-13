using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class FlashCommand : CommandBase
{
    public override string[] Aliases => ["flash"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "freerun + speedy + cargo";

    protected override Action<string[]> GetLogicCallback()
    {
        return CommandRegistry.GetCallback<FreerunCommand>()
            + CommandRegistry.GetCallback<MovementCommand>()
            + CommandRegistry.GetCallback<CargoCommand>();
    }
}
