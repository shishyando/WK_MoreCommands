using System.Collections.Generic;
using System.Linq;

namespace MoreCommands.Common;

public static class PrefabsEntities
{
    // GameEntity
    public static List<GameEntity> AllGameEntities()
    {
        return CL_GameManager.gMan?.gameEntityPrefabs ?? [];
    }
    // Filtered GameEntity
    public static List<GameEntity> FilterGameEntities(string filter)
    {
        return AllGameEntities().FindAll(x => { return Helpers.Substr(x.entityPrefabID, filter); });
    }

    // All Names
    public static List<string> AllGameEntityNames()
    {
        return [.. AllGameEntities().Select(x => {return x.entityPrefabID.ToLower();})];
    }
    public static string JoinAllGameEntityNames(string delimiter = "\n- ")
    {
        return string.Join(delimiter, AllGameEntityNames());
    }

    // Filtered Names
    public static List<string> FilteredGameEntityNames(string filter)
    {
        return [.. FilterGameEntities(filter).Select(x => {return x.entityPrefabID.ToLower();})];
    }
    public static string JoinFilteredGameEntityNames(string filter, string delimiter = "\n- ")
    {
        return string.Join(delimiter, FilteredGameEntityNames(filter));
    }

    // Any
    public static GameEntity AnyGameEntity(string filter)
    {
        return FilterGameEntities(filter).FirstOrDefault();
    }
    public static string AnyGameEntityName(string filter)
    {
        return AnyGameEntity(filter)?.name?.ToLower() ?? null;
    }

}
