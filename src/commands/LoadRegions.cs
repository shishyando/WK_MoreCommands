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
            if (regions == null) return;
            CL_GameManager.gMan.LoadLevels(regions.Data().SelectMany(x => x.GetLevels(null)).Select(x => x.name.ToLower()).ToArray());
        };
    }

}
