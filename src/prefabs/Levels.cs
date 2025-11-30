using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreCommands.HandleProviders;

public class Levels : HandleProvider<M_Level>
{
    public override Func<M_Level, string> Name {get;} = x => x?.name?.ToLower() ?? null;

    private List<M_Level> _levels = [];
    private readonly Dictionary<M_Region, List<M_Level>> _regions = [];
    private readonly Dictionary<M_Subregion, List<M_Level>> _subregions = [];

    public void Initialize()
    {
        _levels = CL_AssetManager.GetFullCombinedAssetDatabase().levelPrefabs
            .Select(obj => obj.GetComponent<M_Level>())
            .Where(c => c != null)
            .Distinct()
            .ToList()
        ;
        foreach (M_Level level in _levels)
        {
            if (level?.region != null)
            {
                if (!_regions.TryGetValue(level.region, out var regionalList))
                {
                    regionalList = [];
                    _regions[level.region] = regionalList;
                }
                regionalList.Add(level);
            }

            if (level?.subRegion != null)
            {
                if (!_subregions.TryGetValue(level.subRegion, out var subregionalList))
                {
                    subregionalList = [];
                    _subregions[level.subRegion] = subregionalList;
                }
                subregionalList.Add(level);
            }
        }
    }

    public override Handle<M_Level> Handle()
    {
        if (_levels.Count == 0) Initialize();
        return new(_levels, Name, base.Finalizer);
    }
    public Handle<M_Level> Regional(M_Region region)
    {
        if (_levels.Count == 0) Initialize();
        return new(_regions[region], Name, base.Finalizer);
    }
    public Handle<M_Level> Subregional(M_Subregion subregion)
    {
        if (_levels.Count == 0) Initialize();
        return new(_subregions[subregion], Name, base.Finalizer);
    }

}
