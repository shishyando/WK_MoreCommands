using System.Linq;
using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Commands;

public sealed class MarkCommand : CommandBase
{
    public override string[] Aliases => ["mark"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Save a run-local position mark; the default name is 'default'";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1
            && (args.Length == 0 || PositionMarks.IsValidName(PositionMarks.NormalizeName(args[0])));
        error = valid ? null : "Usage: mark [name without spaces or semicolons]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteMarkName)
            .AutocompleteValidator(ValidateMarkName);
    }

    public override void Execute(string[] args)
    {
        string name = PositionMarks.NormalizeName(args.FirstOrDefault());
        if (!PositionMarks.IsValidName(name))
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Invalid mark name: {name}");
            return;
        }

        PositionMarks.Save(name);
        Accessors.CommandConsoleAccessor.EchoToConsole($"Saved mark {Colors.Highlighted(name)} at {WorldCommandHelpers.FormatVector(ENT_Player.playerObject.transform.position)}");
    }

    public override void OnExit()
    {
        PositionMarks.Clear();
    }

    private static void AutocompleteMarkName(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg > 0) autocomplete.Reject();
    }

    private static void ValidateMarkName(CommandConsole.CommandValidator validator)
    {
        if (validator.activeArg != 0 || !PositionMarks.IsValidName(validator.ArgumentAt(validator.activeArg)))
        {
            validator.Reject();
        }
    }
}

public sealed class WarpCommand : CommandBase
{
    public override string[] Aliases => ["warp"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Teleport to a run-local mark; the default name is 'default'";
    public override bool EnablesCheatsOnUse => true;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1
            && (args.Length == 0 || PositionMarks.TryGet(args[0], out _));
        error = valid ? null : "Usage: warp [saved mark name]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteSavedMark)
            .AutocompleteValidator(ValidateSavedMark)
            .OverValue(() => PositionMarks.Names());
    }

    public override void Execute(string[] args)
    {
        string name = PositionMarks.NormalizeName(args.FirstOrDefault());
        if (!PositionMarks.TryGet(name, out PositionMarks.Mark mark))
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"No saved mark named {name}");
            return;
        }

        ENT_Player.playerObject.Teleport(mark.Position, mark.Rotation);
        WorldCommandHelpers.MoveDeathgooBelowPlayerIfNeeded();
        Accessors.CommandConsoleAccessor.EchoToConsole($"Warped to {Colors.Highlighted(mark.Name)} ({mark.Level})");
    }

    private static void AutocompleteSavedMark(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg == 0)
        {
            autocomplete.FromArray(PositionMarks.Names());
            return;
        }

        autocomplete.Reject();
    }

    private static void ValidateSavedMark(CommandConsole.CommandValidator validator)
    {
        if (validator.activeArg != 0 || !PositionMarks.TryGet(validator.ArgumentAt(validator.activeArg), out _))
        {
            validator.Reject();
        }
    }
}

public sealed class MarksCommand : CommandBase
{
    public override string[] Aliases => ["marks"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List run-local warp marks";
    public override bool EnablesCheatsOnUse => false;

    public override void Execute(string[] args)
    {
        PositionMarks.Mark[] marks = PositionMarks.All();
        if (marks.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole("No saved marks");
            return;
        }

        Accessors.CommandConsoleAccessor.EchoToConsole(marks.Join(mark =>
            $"- {Colors.Highlighted(mark.Name)}: {mark.Level} [{WorldCommandHelpers.FormatVector(mark.Position)}]", "\n"));
    }
}

public sealed class UnmarkCommand : CommandBase
{
    public override string[] Aliases => ["unmark"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "Delete a run-local warp mark by name";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length == 1 && PositionMarks.TryGet(args[0], out _);
        error = valid ? null : "Usage: unmark <saved mark name>";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(WarpCommandAutocomplete)
            .AutocompleteValidator(WarpCommandValidator);
    }

    public override void Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole("Usage: unmark [name]");
            return;
        }

        string name = PositionMarks.NormalizeName(args[0]);
        if (PositionMarks.Remove(name))
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Deleted mark {Colors.Highlighted(name)}");
            return;
        }

        Accessors.CommandConsoleAccessor.EchoToConsole($"No saved mark named {name}");
    }

    private static void WarpCommandAutocomplete(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg == 0)
        {
            autocomplete.FromArray(PositionMarks.Names());
            return;
        }

        autocomplete.Reject();
    }

    private static void WarpCommandValidator(CommandConsole.CommandValidator validator)
    {
        if (validator.activeArg != 0 || !PositionMarks.TryGet(validator.ArgumentAt(validator.activeArg), out _))
        {
            validator.Reject();
        }
    }
}
