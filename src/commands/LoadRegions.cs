using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class LoadRegionCommand : CommandBase
{
    public override string[] Aliases => ["loadregions"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Load some region levels by region name, filtered by `arg`";
    public override bool CheatsOnly => false;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Handle<M_Region> regions = Prefabs.RegionProvider().FromCommandMany(args);
            if ((regions?.Count() ?? 0) == 0) return;
            try
            {
                string[] levels = regions.Data().SelectMany(x => x.GetLevels(null)).Select(x => x.name.ToLower()).ToArray();
                if (levels.Length == 0)
                {
                    Accessors.CommandConsoleAccessor.EchoToConsole($"Failed to generate levels for regions:\n- {regions.Join()}");
                    return;
                }
                Accessors.CommandConsoleAccessor.EchoToConsole($"Loading regions:\n- {regions.Join()}\n");
                Accessors.CommandConsoleAccessor.EchoToConsole($"levels:\n- {string.Join("\n- ", levels)}");
                CL_GameManager.gMan.LoadLevels(levels);
            }
            catch
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Failed to generate levels for regions:\n- {regions.Join()}");
                return;
            }
        };
    }

}
