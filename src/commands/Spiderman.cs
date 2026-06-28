using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

public sealed class SpidermanCommand : CommandBase
{
    public override string[] Aliases => ["spiderman"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Give infinite barnacle hooks to both hands";
    public override bool EnablesCheatsOnUse => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            CommandConsole.instance.ExecuteCommand("left item_barnaclehook_infinite; right item_barnaclehook_infinite", recordHistory: true);
        };
    }
}
