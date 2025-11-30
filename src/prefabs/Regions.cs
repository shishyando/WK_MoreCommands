using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreCommands.HandleProviders;

public class Regions : HandleProvider<M_Region>
{
    public override Func<M_Region, string> Name {get;} = x => x?.name?.ToLower();
    private List<M_Region> _regions = [];

    public void Initialize()
    {
        _regions = CL_AssetManager.GetFullCombinedAssetDatabase().regionAssets
            .Where(x => x != null)
            .Distinct()
            .ToList()
        ;
    }

    public override Handle<M_Region> Handle()
    {
        if (_regions.Count == 0) Initialize();
        return new(_regions, Name, base.Finalizer);
    }
}
