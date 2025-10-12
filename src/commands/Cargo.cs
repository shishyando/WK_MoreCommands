using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class CargoCommand : CommandBase
{
    public override string[] Aliases => ["cargo"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "max backstrength";
    public override bool CheatsOnly => true;

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            PerkChanger.MaxOutPerk("Perk_BackStrengtheners");
        };
    }
}
