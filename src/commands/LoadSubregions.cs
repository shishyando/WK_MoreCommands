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
            if (subregions == null) return;
            CL_GameManager.gMan.LoadLevels(subregions.Data().SelectMany(x => x.levels).Select(x => x.name.ToLower()).ToArray());
        };
    }

}
