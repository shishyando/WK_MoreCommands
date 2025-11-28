using System.Collections.Generic;
using System.Linq;

namespace MoreCommands.Common;

public static class PrefabsEntities
{
    // GameEntity
    // All
    public static List<GameEntity> AllGameEntities()
    {
        return CL_GameManager.gMan?.gameEntityPrefabs ?? [];
    }
    // Filtered
    public static List<GameEntity> FilterGameEntities(string filter)
    {
        return AllGameEntities().FindAll(x => { return Helpers.Substr(x.entityPrefabID, filter.ToLower()); });
    }
    // Any
    public static GameEntity AnyGameEntity(string filter)
    {
        return FilterGameEntities(filter).FirstOrDefault();
    }
 
    // Names
    // All
    public static List<string> AllGameEntityNames()
    {
        return [.. AllGameEntities().Select(x => {return x.entityPrefabID.ToLower();})];
    }
    // All Joined
    public static string JoinAllGameEntityNames(string delimiter = "\n- ")
    {
        return string.Join(delimiter, AllGameEntityNames());
    }
    // Filtered
    public static List<string> FilteredGameEntityNames(string filter)
    {
        return [.. FilterGameEntities(filter).Select(x => {return x.entityPrefabID.ToLower();})];
    }
    // Filtered Joined
    public static string JoinFilteredGameEntityNames(string filter, string delimiter = "\n- ")
    {
        return string.Join(delimiter, FilteredGameEntityNames(filter));
    }
    // Any
    public static string AnyGameEntityName(string filter)
    {
        return AnyGameEntity(filter)?.entityPrefabID?.ToLower() ?? null;
    }
    
    public static GameEntity GameEntityForCommandFromArgs(string[] args)
    {
        if (args.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Available entities:\n- {JoinAllGameEntityNames()}");
            return null;
        }
        var e = AnyGameEntity(args[0]);
        if (e == null)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"No such entity: {args[0]}");
            return null;
        }
        return e;
    }
}
