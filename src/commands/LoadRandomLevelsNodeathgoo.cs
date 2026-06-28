using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

public sealed class LoadRandomLevelsNoDeathgooCommand : CommandBase
{
    public override string[] Aliases => ["loadrandomlevels_nodeathgoo"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Load random levels, then send deathgoo away";
    public override bool EnablesCheatsOnUse => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            CommandConsole.instance.ExecuteCommand("loadrandomlevels; deathgoo-goaway", recordHistory: true);
        };
    }
}
