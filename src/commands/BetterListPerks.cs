using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class BetterListPerksCommand : CommandBase
{
    public override string[] Aliases => ["lp"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List perks info, filtered by `arg`";
    public override bool CheatsOnly => false;

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            foreach (Perk perk in CL_AssetManager.GetFullCombinedAssetDatabase().perkAssets)
            {
                if (args.Length == 0 || Has(perk.name, args[0]) || Has(perk.description, args[0]) || Has(perk.id, args[0]))
                {
                    Accessors.CommandConsoleAccessor.EchoToConsole($"- {perk.GetTitle()} (<color=grey>{perk.id}</color>)\n{perk.GetDescription(includeFlavor: false)}</color></color></color>\n"); // in case some tags were left unclosed
                }
            }
        };
    }

    private bool Has(string text, string check)
    {
        return text?.IndexOf(check, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
