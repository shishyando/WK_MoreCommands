using System;
using System.Collections.Generic;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class MovementCommand : CommandBase
{
    public override string[] Aliases => ["speedy"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "get some movement perks `arg` times (1 by default)";
    public override bool CheatsOnly => true;

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabled();
            ENT_Player player = ENT_Player.playerObject;
            if (player == null) return;
            for (int i = 0; i < ArgParse.GetMult(args, 1); ++i)
            {
                foreach (var perkToAdd in MovementPerks)
                {
                    for (int j = 0; j < perkToAdd.Value; ++j)
                    {
                        player.AddPerk([perkToAdd.Key]);
                    }
                }
            }
        };
    }

    public static Dictionary<string, int> MovementPerks = new()
    {
        ["perk_armoredplating"] = 5,
        ["perk_elasticlimbs"] = 2,
        ["perk_metabolicstasis"] = 2,
        ["perk_pulseorgan"] = 10,
        ["perk_rabbitdna"] = 1,
        ["perk_somaticpainkillers"] = 2,
        ["perk_systemreorganization"] = 4,
    };
}
