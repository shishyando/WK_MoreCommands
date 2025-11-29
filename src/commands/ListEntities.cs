using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListEntitiesCommand : CommandBase
{
    public override string[] Aliases => ["listentities"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List entities, filtered by `arg`";
    public override bool CheatsOnly => false;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"- {PrefabsEntities.JoinFilteredGameEntityNames(args.FirstOrDefault() ?? "")}");
        };
    }

}
