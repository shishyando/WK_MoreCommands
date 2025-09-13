
using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public static class CargoCommand
{
    public static string[] Aliases => ["cargo"];
    public static CommandTag Tag => CommandTag.Player;
    public static string Description => "backstrength times `arg`, (9 by default)";

    public static Action<string[]> GetCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabld();
            ENT_Player player = ENT_Player.playerObject;
            if (player == null) return;

            for (int i = 0; i < ArgParse.GetMult(args, 9); ++i) player.AddPerk(["perk_backstrengtheners"]);
        };
    }
}
