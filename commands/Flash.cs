
using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public static class FlashCommand
{
    public static string[] Aliases => ["flash"];
    public static CommandTag Tag => CommandTag.Player;
    public static string Description => "freerun + speedy + cargo";
    public static bool Enabled = true;

    public static Action<string[]> GetCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabld();
            // Reuse freerun basics
            FreerunCommand.ApplyFreerunState(true);
            // Add speedy perks and cargo capacity
            MovementCommand.GetCallback().Invoke([]);
            CargoCommand.GetCallback().Invoke([]);
        };
    }
}
