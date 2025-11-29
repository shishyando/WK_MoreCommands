using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class BetterListPerksCommand : CommandBase
{
    public override string[] Aliases => ["lp"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List perks info, filtered by `arg`";
    public override bool CheatsOnly => false;

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
