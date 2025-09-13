
using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

// explore = freerun + noclip
public static class ExploreCommand
{
    public static string[] Aliases => ["explore"];
    public static CommandTag Tag => CommandTag.Player;
    public static bool Enabled;

    public static Action<string[]> GetCallback()
    {
        return args =>
        {
            CommandHelpers.UpdateEnabled(ref Enabled, args);
            if (Enabled)
            {
                Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabld();
            }
            FreerunCommand.ApplyFreerunState(Enabled);
            ENT_Player player = ENT_Player.playerObject;
            player?.Noclip(CommandHelpers.WhenEnabled(Enabled));
        };
    }
}
