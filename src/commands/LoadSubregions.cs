using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class LoadSubregionCommand : CommandBase
{
    public override string[] Aliases => ["loadsubregions"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Load all subregion levels by subregion name, filtered by `arg`";
    public override bool CheatsOnly => false;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Handle<M_Subregion> subregions = Prefabs.SubregionProvider().FromCommandMany(args);
            if ((subregions?.Count() ?? 0) == 0) return;
            string[] levels = subregions.Data().SelectMany(x => x.levels).Select(x => x.name.ToLower()).ToArray();
            if (levels.Length == 0)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Failed to get levels for subregions:\n- {subregions.Join()}");
                return;
            }
            Accessors.CommandConsoleAccessor.EchoToConsole($"Loading subregions:\n- {subregions.Join()}");
            Accessors.CommandConsoleAccessor.EchoToConsole($"levels:\n- {string.Join("\n- ", levels)}");
            CL_GameManager.gMan.LoadLevels(levels);
        };
    }

}
