using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreCommands.HandleProviders;

public class Subregions : HandleProvider<M_Subregion>
{
    public override Func<M_Subregion, string> Name {get;} = x => x?.name?.ToLower();
    private List<M_Subregion> _subregions = [];

    public void Initialize()
    {
        _subregions = CL_AssetManager.GetFullCombinedAssetDatabase().subRegionAssets
            .Where(x => x != null && (x.levels?.Count ?? 0) > 0)
            .Distinct()
            .ToList()
        ;
    }

    public override Handle<M_Subregion> Handle()
    {
        if (_subregions.Count == 0) Initialize();
        return new(_subregions, Name, base.Finalizer);
    }
}
