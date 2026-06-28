using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

public sealed class LoadRandomLevelsSpidermanCommand : CommandBase
{
    public override string[] Aliases => ["loadrandomlevels_spiderman"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Load random levels, then give infinite barnacle hooks to both hands";
    public override bool EnablesCheatsOnUse => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            CommandConsole.instance.ExecuteCommand("loadrandomlevels; spiderman", recordHistory: true);
        };
    }
}
