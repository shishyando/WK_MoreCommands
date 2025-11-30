using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class LoadLevelsCommand : CommandBase
{
    public override string[] Aliases => ["loadlevels"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Load levels by its names with substring search";
    public override bool CheatsOnly => false;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            var levels = Prefabs.LevelProvider().FromCommandMany(args);
            if (levels.Count() == 0) return;
            CL_GameManager.gMan.LoadLevels(levels.Names());
        };
    }

}
