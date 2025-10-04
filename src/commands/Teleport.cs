using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class TeleportCommand : CommandBase
{
    public override string[] Aliases => ["tp"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "teleport to `arg`, no `arg` = list locations";
    public override bool CheatsOnly => true;

    private static readonly Dictionary<string, string> Locations = new() {
        { "intro", "m1_intro_01"},
        { "silos_exp_perk", "m1_silos_safearea_01" },
        { "prepipes", "campaign_interlude_silo_to_pipeworks" },
        { "elevator", "campaign_interlude_pipeworks_to_habitation_01" },
        { "pier", "m3_habitation_pier_entrance_01" },
        { "abyss", "m3_habitation_lab_ending" },
        // shortcuts? pier otherside? obelisk?
        // need to preload levels?
    };


    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabled();
            ENT_Player player = ENT_Player.playerObject;
            if (args.Length == 0)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Available locations:\n-{Locations.Keys.Join(delimiter: "\n-")}");
            }
            else if (Locations.ContainsKey(args[0].ToLower()))
            {
                WorldLoader.instance.TeleportPlayerToTargetLevel([Locations[args[0].ToLower()]]);
            }
            else
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Invalid arguments for tp command: {args.Join()}");
            }
        };
    }
}
