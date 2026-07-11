using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class SpeedyPerksCommand : CommandBase
{
    public override string[] Aliases => ["speedyperks"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Max out the curated MoreCommands perk set";
    public override bool EnablesCheatsOnUse => true;

    private static readonly string[] PerkIds = [
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

    public override void Execute(string[] args)
    {
        foreach (string perkId in PerkIds)
        {
            PerkChanger.MaxOutPerk(perkId);
        }
        Accessors.CommandConsoleAccessor.EchoToConsole("MoreCommands perk set maxed out");
    }

}
