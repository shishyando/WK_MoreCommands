using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class CargoCommand : CommandBase
{
    public override string[] Aliases => ["cargo"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Max out the Back Strengtheners perk";
    public override bool EnablesCheatsOnUse => true;

    public override void Execute(string[] args)
    {
        PerkChanger.MaxOutPerk("Perk_BackStrengtheners");
        Accessors.CommandConsoleAccessor.EchoToConsole("Back Strengtheners maxed out");
    }
}
