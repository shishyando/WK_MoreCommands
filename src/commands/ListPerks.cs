using System.Collections.Generic;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class BetterListPerksCommand : CommandBase
{
    public override string[] Aliases => ["lp", "mclistperks"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List perk titles, IDs, and descriptions, optionally filtered by substring";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1;
        error = valid ? null : "Usage: mclistperks [perk ID, title, or description filter]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompletePerks)
            .AutocompleteValidator(AutocompleteHelpers.ValidateOptionalSingleFilter);
    }

    private static void AutocompletePerks(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg == 0)
        {
            autocomplete.FromArrayWithDesc(PerkEntries());
            return;
        }

        autocomplete.Reject();
    }

    private static IReadOnlyList<(string name, string desc)> PerkEntries()
    {
        return [.. CL_AssetManager.GetFullCombinedAssetDatabase().perkAssets
            .Where(perk => perk != null && !string.IsNullOrWhiteSpace(perk.id))
            .Select(perk => (perk.id.ToLowerInvariant(), perk.GetTitle()))];
    }

    public override void Execute(string[] args)
    {
        Perk[] perks = [.. CL_AssetManager.GetFullCombinedAssetDatabase().perkAssets
            .Where(perk => perk != null)
            .Where(perk => args.Length == 0 || PerkMatches(perk, args[0]))];
        if (perks.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole("No matching perks");
            return;
        }

        foreach (Perk perk in perks)
        {
            string perkDescription = perk.GetDescription(includeFlavor: false, includeColorTags: false);
            string info = $"- {perk.GetTitle()} ({Colors.Highlighted(perk.id)})\n{perkDescription}\n";
            Accessors.CommandConsoleAccessor.EchoToConsole(info);
        }
    }

    private static bool PerkMatches(Perk perk, string value)
    {
        return Helpers.Substr(perk.title, value)
            || Helpers.Substr(perk.description, value)
            || Helpers.Substr(perk.id, value);
    }
}
