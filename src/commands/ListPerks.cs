using System;
using System.Collections.Generic;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class BetterListPerksCommand : CommandBase
{
    public override string[] Aliases => ["lp", "mclistperks"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "Enhanced perk list with titles, ids, descriptions, and filtering";
    public override bool EnablesCheatsOnUse => false;

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompletePerks)
            .AutocompleteValidator(ValidatePerks);
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

    private static void ValidatePerks(CommandConsole.CommandValidator validator)
    {
        string value = validator.ArgumentAt(validator.activeArg);
        if (validator.activeArg != 0 || !PerkMatches(value))
        {
            validator.Reject();
        }
    }

    private static IReadOnlyList<(string name, string desc)> PerkEntries()
    {
        return [.. CL_AssetManager.GetFullCombinedAssetDatabase().perkAssets
            .Where(perk => perk != null && !string.IsNullOrWhiteSpace(perk.id))
            .Select(perk => (perk.id.ToLower(), perk.GetTitle()))];
    }

    private static bool PerkMatches(string value)
    {
        return !string.IsNullOrWhiteSpace(value)
            && CL_AssetManager.GetFullCombinedAssetDatabase().perkAssets
                .Where(perk => perk != null)
                .Any(perk => Helpers.Substr(perk.name, value)
                    || Helpers.Substr(perk.description, value)
                    || Helpers.Substr(perk.id, value));
    }

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            foreach (Perk perk in CL_AssetManager.GetFullCombinedAssetDatabase().perkAssets)
            {
                if (args.Length == 0 || Helpers.Substr(perk.name, args[0]) || Helpers.Substr(perk.description, args[0]) || Helpers.Substr(perk.id, args[0]))
                {
                    string perkDescription = perk.GetDescription(includeFlavor: false);
                    string info = $"- {perk.GetTitle()} ({Colors.Highlighted(perk.id)})\n{perkDescription}</color></color></color>\n"; // in case some tags were left unclosed by game
                    Accessors.CommandConsoleAccessor.EchoToConsole(info);
                }
            }
        };
    }
}
