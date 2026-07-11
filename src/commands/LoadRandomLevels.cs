using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class LoadRandomLevelsCommand : CommandBase
{
    public override string[] Aliases => ["loadrandomlevels"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Start the level tester with a random non-empty set of levels";
    public override bool EnablesCheatsOnUse => false;

    public override void Execute(string[] args)
    {
        M_Level.LevelAssetHolder[] levels = [.. CL_AssetManager.GetFullCombinedAssetDatabase().levelAssets
            .Where(level => !string.IsNullOrWhiteSpace(level?.id))];
        if (levels.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole("No levels are available");
            return;
        }

        int count = UnityEngine.Random.Range(1, levels.Length + 1);
        string[] levelNames = levels
            .OrderBy(x => UnityEngine.Random.value)
            .Take(count)
            .Select(level => level.id.ToLowerInvariant())
            .ToArray();

        Accessors.CommandConsoleAccessor.EchoToConsole($"Loading random levels:\n- {string.Join("\n- ", levelNames)}");
        CL_GameManager.gMan.LoadLevels(levelNames);
    }
}
