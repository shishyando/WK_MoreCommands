using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class LoadRegionCommand : CommandBase
{
    public override string[] Aliases => ["loadregions"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Start the level tester with levels from one or more exact region names";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length > 0
            && args.All(arg => AutocompleteHelpers.HasExactMatch(Prefabs.Regions, arg));
        error = valid ? null : "Usage: loadregions <exact region name ...>";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(autocomplete => AutocompleteHelpers.VariadicExactFrom(autocomplete, Prefabs.Regions))
            .AutocompleteValidator(validator => AutocompleteHelpers.ValidateVariadicExactFrom(validator, Prefabs.Regions));
    }

    public override void Execute(string[] args)
    {
        Handle<M_Region> regions = Prefabs.RegionProvider().FromCommandMany(args);
        if ((regions?.Count() ?? 0) == 0) return;
        string[] levels;
        try
        {
            levels = [.. regions.Data()
                .SelectMany(region => region.GetLevels(null))
                .Where(reference => !string.IsNullOrWhiteSpace(reference?.id))
                .Select(reference => reference.id.ToLowerInvariant())];
        }
        catch (Exception exception)
        {
            Plugin.Beep.LogWarning($"Failed to generate region levels: {exception}");
            Accessors.CommandConsoleAccessor.EchoToConsole($"Failed to generate levels for regions:\n- {regions.Join()}");
            return;
        }

        if (levels.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Failed to generate levels for regions:\n- {regions.Join()}");
            return;
        }
        Accessors.CommandConsoleAccessor.EchoToConsole($"Loading regions:\n- {regions.Join()}\n");
        Accessors.CommandConsoleAccessor.EchoToConsole($"levels:\n- {string.Join("\n- ", levels)}");
        CL_GameManager.gMan.LoadLevels(levels);
    }

}
