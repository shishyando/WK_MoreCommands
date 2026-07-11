using System;
using System.Linq;

namespace MoreCommands.HandleProviders;

public class Subregions : HandleProvider<M_Subregion>
{
    public override Func<M_Subregion, string> Name { get; } = subregion => subregion?.name;

    public override Handle<M_Subregion> Handle()
    {
        return new(
            CL_AssetManager.GetFullCombinedAssetDatabase().subRegionAssets
                .Where(subregion => subregion != null && (subregion.levelReferences?.Count ?? 0) > 0)
                .Distinct(),
            Name);
    }
}
