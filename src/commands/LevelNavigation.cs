using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

public abstract class LevelNavigationCommandBase : CommandBase
{
    protected abstract int Direction { get; }
    protected abstract string Verb { get; }

    public override CommandTag Tag => CommandTag.Player;
    public override bool EnablesCheatsOnUse => true;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1
            && (args.Length == 0 || (int.TryParse(args[0], out int count) && count >= 1));
        error = valid ? null : $"Usage: {Aliases[0]} [positive level count]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteHelpers.OptionalSingleFloat)
            .AutocompleteValidator(ValidateOptionalPositiveInt);
    }

    public override void Execute(string[] args)
    {
        int count = 1;
        if (args.Length > 0 && (!int.TryParse(args[0], out count) || count < 1))
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Invalid {Verb} count: {args[0]}");
            return;
        }

        WorldLoader.BranchInfo branch = WorldLoader.instance?.GetCurrentBranch();
        if (branch?.levelTracker == null || branch.levelTracker.Count == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole("No loaded levels found");
            return;
        }

        int current = WorldCommandHelpers.GetCurrentLevelIndex(branch);
        long requested = (long)current + (long)Direction * count;
        int target = (int)Math.Min(Math.Max(requested, 0L), branch.levelTracker.Count - 1L);
        if (target == current)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Already at {Verb} edge of current branch");
            return;
        }

        if (WorldCommandHelpers.TeleportToLevel(branch.levelTracker[target], out string message))
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"{message} [{target + 1}/{branch.levelTracker.Count}]");
            return;
        }

        Accessors.CommandConsoleAccessor.EchoToConsole(message);
    }

    private static void ValidateOptionalPositiveInt(CommandConsole.CommandValidator validator)
    {
        if (validator.activeArg != 0
            || !int.TryParse(validator.ArgumentAt(validator.activeArg), out int value)
            || value < 1)
        {
            validator.Reject();
        }
    }
}

public sealed class NextLevelCommand : LevelNavigationCommandBase
{
    public override string[] Aliases => ["nextlevel"];
    public override string Description => "Teleport forward by an optional number of loaded levels";
    protected override int Direction => 1;
    protected override string Verb => "next";
}

public sealed class PrevLevelCommand : LevelNavigationCommandBase
{
    public override string[] Aliases => ["prevlevel"];
    public override string Description => "Teleport backward by an optional number of loaded levels";
    protected override int Direction => -1;
    protected override string Verb => "previous";
}
