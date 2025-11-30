using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListSubregionsCommand : CommandBase
{
    public override string[] Aliases => ["listsubregions"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List subregions, filtered by `arg`";
    public override bool CheatsOnly => false;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"- {Prefabs.Subregions().Filter(args.FirstOrDefault() ?? "").Join()}");
        };
    }

}
