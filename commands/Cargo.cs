using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class CargoCommand : CommandBase
{
    public override string[] Aliases => ["cargo"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "backstrength times `arg`, (9 by default)";

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabled();
            ENT_Player player = ENT_Player.playerObject;
            if (player == null) return;

            for (int i = 0; i < ArgParse.GetMult(args, 9); ++i) player.AddPerk(["perk_backstrengtheners"]);
        };
    }
}
