using System;
using UnityEngine;

namespace MoreCommands.Common;

public static class WorldCommandHelpers
{
    public static string FormatVector(Vector3 value)
    {
        return $"{value.x:F2} {value.y:F2} {value.z:F2}";
    }

    public static string FormatRotation(Quaternion value)
    {
        Vector3 euler = value.eulerAngles;
        return $"{euler.x:F1} {euler.y:F1} {euler.z:F1}";
    }

    public static string LevelName(WorldLoader.LevelInfo level)
    {
        return level?.GetLevel()?.levelName
            ?? level?.GetLevel()?.name?.Replace("(Clone)", "").Replace("(clone)", "")
            ?? "unknown";
    }

    public static string RegionName(WorldLoader.LevelInfo level)
    {
        return level?.GetLevel()?.region?.name ?? "inter_region";
    }

    public static string SubregionName(WorldLoader.LevelInfo level)
    {
        return level?.GetLevel()?.subRegion?.name ?? "inter_region";
    }

    public static WorldLoader.LevelInfo GetCurrentLevel()
    {
        ENT_Player player = ENT_Player.playerObject;
        WorldLoader loader = WorldLoader.instance;
        if (player == null || loader == null) return loader?.GetCurrentLevel();

        return WorldLoader.GetClosestLevelToPosition(player.transform.position) ?? loader.GetCurrentLevel();
    }

    public static int GetCurrentLevelIndex(WorldLoader.BranchInfo branch)
    {
        if (branch?.levelTracker == null || branch.levelTracker.Count == 0) return -1;

        WorldLoader.LevelInfo current = GetCurrentLevel();
        int index = current == null ? -1 : branch.levelTracker.IndexOf(current);
        if (index >= 0) return index;

        index = branch.currentLevel == null ? -1 : branch.levelTracker.IndexOf(branch.currentLevel);
        return index >= 0 ? index : 0;
    }

    public static bool TeleportToLevel(WorldLoader.LevelInfo level, out string message)
    {
        ENT_Player player = ENT_Player.playerObject;
        M_Level target = level?.GetLevel();
        if (player == null)
        {
            message = "No player found";
            return false;
        }
        if (target == null)
        {
            message = "No target level found";
            return false;
        }

        Transform destination = target.spawnPosition;
        if (destination != null)
        {
            player.Teleport(destination.position, destination.rotation);
        }
        else
        {
            destination = target.GetLevelEntrance();
            if (destination == null)
            {
                message = $"No spawn or entrance found for {LevelName(level)}";
                return false;
            }

            player.Teleport(destination.position + Vector3.up * 5f);
        }

        MoveDeathgooBelowPlayerIfNeeded();
        message = $"Teleported to {Colors.Highlighted(LevelName(level))}";
        return true;
    }

    public static void MoveDeathgooBelowPlayerIfNeeded()
    {
        ENT_Player player = ENT_Player.playerObject;
        DEN_DeathFloor deathgoo = DEN_DeathFloor.instance;
        if (player != null && deathgoo != null && deathgoo.transform.position.y > player.transform.position.y - 20f)
        {
            deathgoo.Teleport(player.transform.position - Vector3.up * 20f);
        }
    }

}
