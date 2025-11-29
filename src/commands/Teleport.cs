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
        { "exp", "m1_silos_safearea_01" },
        { "prepipes", "campaign_interlude_silo_to_pipeworks" },
        { "elevator", "campaign_interlude_pipeworks_to_habitation_01" },
        { "teeth", "m3_habitation_shaft_intro" },
        { "pier", "m3_habitation_pier_entrance_01" },
        { "abyss", "m3_habitation_lab_ending" },
        // shortcuts? pier otherside? obelisk?
        // need to preload levels?
    };


    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            ENT_Player player = ENT_Player.playerObject;
            if (args.Length == 0)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Available locations:\n- {Locations.Keys.Join(delimiter: "\n- ")}");
                return;
            }
            List<string> destinations = [];
            foreach (var tp in Locations)
            {
                if (tp.Key.Contains(args[0].ToLower()))
                {
                    destinations.Add(tp.Value);
                }
            }
            if (destinations.Count == 0)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"No such tp option, available options:\n- {string.Join("\n- ", Locations.Keys)}");
                return;
            }
            if (destinations.Count > 1)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Ambiguous place to tp: {args[0]} maps to {string.Join(", ", destinations)}\n teleporting to first");
            }
            WorldLoader.instance.TeleportPlayerToTargetLevel([destinations[0]]);
            Accessors.CommandConsoleAccessor.EchoToConsole($"Teleported to {Colors.Highlighted(destinations[0])}");
        };
    }
}
