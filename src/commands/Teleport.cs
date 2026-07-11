using System;
using System.Collections.Generic;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class TeleportCommand : CommandBase
{
    public override string[] Aliases => ["tp"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Teleport to an exact named shortcut; no argument lists shortcuts";
    public override bool EnablesCheatsOnUse => true;

    private static readonly Dictionary<string, string> Locations = new(StringComparer.OrdinalIgnoreCase) {
        { "intro", "m1_intro_01"},
        { "exp", "m1_silos_safearea_01" },
        { "prepipes", "campaign_interlude_silo_to_pipeworks" },
        { "teeth", "m3_habitation_shaft_intro" },
        { "pier", "m3_habitation_pier_entrance_01" },
        { "abyss", "m3_habitation_lab_ending" },
    };

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1
            && (args.Length == 0 || Locations.ContainsKey(args[0]));
        error = valid ? null : "Usage: tp [location]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteLocation)
            .AutocompleteValidator(ValidateLocation);
    }

    private static void AutocompleteLocation(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg == 0)
        {
            autocomplete.FromArray([.. Locations.Keys]);
            return;
        }

        autocomplete.Reject();
    }

    private static void ValidateLocation(CommandConsole.CommandValidator validator)
    {
        if (validator.activeArg != 0 || !Locations.ContainsKey(validator.ArgumentAt(validator.activeArg)))
        {
            validator.Reject();
        }
    }


    public override void Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Available locations:\n- {string.Join("\n- ", Locations.Keys)}");
            return;
        }

        WorldLoader.BranchInfo branch = WorldLoader.instance?.GetCurrentBranch();
        if (branch?.levelTracker == null)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole("No loaded levels found");
            return;
        }

        string levelId = Locations[args[0]];
        WorldLoader.LevelInfo destination = branch.levelTracker.FirstOrDefault(level =>
            string.Equals(level?.GetLevelAssetReference()?.id, levelId, StringComparison.OrdinalIgnoreCase));
        if (destination == null)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole(
                $"Shortcut {Colors.Highlighted(args[0])} targets {Colors.Highlighted(levelId)}, which is not loaded");
            return;
        }

        WorldCommandHelpers.TeleportToLevel(destination, out string message);
        Accessors.CommandConsoleAccessor.EchoToConsole(message);
    }
}
