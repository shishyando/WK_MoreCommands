using MoreCommands.Common;

namespace MoreCommands.Commands;

public sealed class LoadRandomLevelsNoDeathgooCommand : CommandBase
{
    public override string[] Aliases => ["loadrandomlevels_nodeathgoo"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Start with random levels, then send deathgoo away after loading";
    public override bool EnablesCheatsOnUse => true;

    public override void Execute(string[] args)
    {
        CommandConsole.instance.ExecuteCommand("loadrandomlevels; deathgoo-goaway", recordHistory: true);
    }
}
