using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ManCommand : CommandBase
{
    public override string[] Aliases => ["man", "mhelp", "morecommandshelp"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "prints MoreCommands with their descriptions ('+' = enables cheats)";
    public override bool CheatsOnly => false;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            foreach (ICommand c in CommandRegistry.GetAllCommands())
            {
                if (args.Length == 0 || c.Aliases.Contains(args[0]))
                {
                    Accessors.CommandConsoleAccessor.EchoToConsole(Colors.FormatCommand(c));
                }
            }
        };
    }
}
