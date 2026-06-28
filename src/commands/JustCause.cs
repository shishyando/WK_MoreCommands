using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

public sealed class JustCauseCommand : CommandBase
{
    public override string[] Aliases => ["justcause"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Give debug handgun to left hand and infinite barnacle hook to right hand";
    public override bool EnablesCheatsOnUse => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            CommandConsole.instance.ExecuteCommand("left item_handgun_debug; right item_barnaclehook_infinite", recordHistory: true);
        };
    }
}
