using System;
using System.Collections.Generic;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class LoadRandomLevelsCommand : CommandBase
{
    public override string[] Aliases => ["loadrandomlevels"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Load completely random levels\nRecommended commands: `godmode`, `deathgoo-goaway`\nHave fun";
    public override bool CheatsOnly => false;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            IEnumerable<M_Level> levels = Prefabs.Levels().Data();
            int count = levels.Count();
            string[] levelNames = levels
                .OrderBy(x => UnityEngine.Random.value)
                .Take(UnityEngine.Random.RandomRangeInt(0, count))
                .Select(x => x.name.ToLower())
                .ToArray();

            Accessors.CommandConsoleAccessor.EchoToConsole($"Loading random levels:\n- {string.Join("\n- ", levelNames)}");
            CL_GameManager.gMan.LoadLevels(levelNames);
        };
    }
}
