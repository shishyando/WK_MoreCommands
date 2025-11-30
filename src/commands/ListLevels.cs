using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListLevelsCommand : CommandBase
{
    public override string[] Aliases => ["listlevels"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List levels, filtered by `arg`";
    public override bool CheatsOnly => false;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"- {Prefabs.Levels().Filter(args.FirstOrDefault() ?? "").Join()}");
        };
    }

}
