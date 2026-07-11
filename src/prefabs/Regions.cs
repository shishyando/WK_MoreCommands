using System;
using System.Linq;

namespace MoreCommands.HandleProviders;

public class Regions : HandleProvider<M_Region>
{
    public override Func<M_Region, string> Name { get; } = region => region?.name;

    public override Handle<M_Region> Handle()
    {
        return new(
            CL_AssetManager.GetFullCombinedAssetDatabase().regionAssets
                .Where(region => region != null)
                .Distinct(),
            Name);
    }
}
