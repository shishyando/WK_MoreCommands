using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class LoadSubregionCommand : CommandBase
{
    public override string[] Aliases => ["loadsubregions"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Start the level tester with levels from one or more exact subregion names";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length > 0
            && args.All(arg => AutocompleteHelpers.HasExactMatch(Prefabs.Subregions, arg));
        error = valid ? null : "Usage: loadsubregions <exact subregion name ...>";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(autocomplete => AutocompleteHelpers.VariadicExactFrom(autocomplete, Prefabs.Subregions))
            .AutocompleteValidator(validator => AutocompleteHelpers.ValidateVariadicExactFrom(validator, Prefabs.Subregions));
    }

    public override void Execute(string[] args)
    {
        Handle<M_Subregion> subregions = Prefabs.SubregionProvider().FromCommandMany(args);
        if ((subregions?.Count() ?? 0) == 0) return;
        string[] levels = [.. subregions.Data()
            .SelectMany(subregion => subregion.levelReferences)
            .Where(reference => !string.IsNullOrWhiteSpace(reference?.id))
            .Select(reference => reference.id.ToLowerInvariant())];
        if (levels.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Failed to get levels for subregions:\n- {subregions.Join()}");
            return;
        }
        Accessors.CommandConsoleAccessor.EchoToConsole($"Loading subregions:\n- {subregions.Join()}");
        Accessors.CommandConsoleAccessor.EchoToConsole($"levels:\n- {string.Join("\n- ", levels)}");
        CL_GameManager.gMan.LoadLevels(levels);
    }

}
