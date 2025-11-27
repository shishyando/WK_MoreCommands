using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class SpeedyPerksCommand : CommandBase
{
    public override string[] Aliases => ["speedyperks"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "get some movement perks";
    public override bool CheatsOnly => true;

    private static List<string> MovementPerks = [
        "Perk_ArmoredPlating",
        "Perk_ElasticLimbs",
        "Perk_MetabolicStasis",
        "Perk_PulseOrgan",
        "Perk_RabbitDNA",
        "Perk_SomaticPainkillers",
        "Perk_SystemReorganization",
        "Perk_AutotomousSkeleton",
        "Perk_AdrenalinePumps",
        "Perk_HeavyStrike",
        "Perk_VelocityAugments",
        "Perk_LatissimusOptimization",
        "Perk_SteadiedStance",
        "Perk_Rho_Blessing_Protection",
        "Perk_Rho_Blessing_Regeneration",
        "Perk_Rho_Blessing_Swift",
        "Perk_Rho_Blessing_Overwhelm",
    ];

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            foreach (var perkId in MovementPerks)
            {
                PerkChanger.MaxOutPerk(perkId);
            }
        };
    }

}
