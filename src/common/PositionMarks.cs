using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreCommands.Common;

public static class PositionMarks
{
    public sealed class Mark
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public string Level { get; set; }
    }

    private static readonly Dictionary<string, Mark> MarksByName = new(System.StringComparer.OrdinalIgnoreCase);

    public static string NormalizeName(string name)
    {
        return string.IsNullOrWhiteSpace(name) ? "default" : name.Trim();
    }

    public static bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name)
            && name.All(c => !char.IsWhiteSpace(c) && c != ';');
    }

    public static void Save(string name)
    {
        ENT_Player player = ENT_Player.playerObject;
        WorldLoader.LevelInfo level = WorldCommandHelpers.GetCurrentLevel();
        string normalized = NormalizeName(name);

        MarksByName[normalized] = new Mark
        {
            Name = normalized,
            Position = player.transform.position,
            Rotation = player.transform.rotation,
            Level = WorldCommandHelpers.LevelName(level),
        };
    }

    public static bool TryGet(string name, out Mark mark)
    {
        return MarksByName.TryGetValue(NormalizeName(name), out mark);
    }

    public static bool Remove(string name)
    {
        return MarksByName.Remove(NormalizeName(name));
    }

    public static void Clear()
    {
        MarksByName.Clear();
    }

    public static string[] Names()
    {
        return [.. MarksByName.Keys.OrderBy(name => name)];
    }

    public static Mark[] All()
    {
        return [.. MarksByName.Values.OrderBy(mark => mark.Name)];
    }
}
