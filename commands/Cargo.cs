
using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

// cargo = backstrength times `arg`, 9 by default
public sealed class CargoCommand : OneshotCommand<CargoCommand>
{
    public override string[] Aliases => ["cargo"];
    public override CommandTag Tag => CommandTag.Player;

    public override Action<string[]> GetCallback()
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
