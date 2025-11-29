using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListItemsCommand : CommandBase
{
    public override string[] Aliases => ["listitems"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List items, filtered by `arg`";
    public override bool CheatsOnly => false;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"- {PrefabsItems.JoinFilteredItemNames(args.FirstOrDefault() ?? "")}");
        };
    }

}
