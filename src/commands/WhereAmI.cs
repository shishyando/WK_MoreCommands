using MoreCommands.Common;

namespace MoreCommands.Commands;

public sealed class WhereAmICommand : CommandBase
{
    public override string[] Aliases => ["whereami", "pos"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Print position, rotation, level, region, branch, seed, and reusable commands";
    public override bool EnablesCheatsOnUse => false;

    public override void Execute(string[] args)
    {
        ENT_Player player = ENT_Player.playerObject;
        WorldLoader loader = WorldLoader.instance;
        WorldLoader.BranchInfo branch = loader?.GetCurrentBranch();
        WorldLoader.LevelInfo level = WorldCommandHelpers.GetCurrentLevel();
        int index = WorldCommandHelpers.GetCurrentLevelIndex(branch);
        string levelName = WorldCommandHelpers.LevelName(level);
        string position = WorldCommandHelpers.FormatVector(player.transform.position);
        string rotation = WorldCommandHelpers.FormatRotation(player.transform.rotation);

        Accessors.CommandConsoleAccessor.EchoToConsole(
            $"Position: {Colors.Highlighted(position)}\n"
            + $"Rotation: {rotation}\n"
            + $"Level: {levelName} [{index + 1}/{branch?.levelTracker?.Count ?? 0}]\n"
            + $"Region: {WorldCommandHelpers.RegionName(level)} / {WorldCommandHelpers.SubregionName(level)}\n"
            + $"Branch: {branch?.id ?? "n/a"}\n"
            + $"Seed: {loader?.GetSeed().ToString() ?? "n/a"}\n"
            + $"teleportplayer {position}\n"
            + $"teleportplayertolevel {levelName}\n"
            + "mark here");
    }
}
