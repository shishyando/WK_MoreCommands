// using System;
// using System.Collections.Generic;
// using System.Linq;
// using HarmonyLib;
// using MoreCommands.Common;

// namespace MoreCommands.Commands;


// public sealed class GiveCommand : CommandBase
// {
//     public override string[] Aliases => ["give"];
//     public override CommandTag Tag => CommandTag.Player;
//     public override string Description => "give item to player";
//     public override bool CheatsOnly => true;

//     private static readonly Dictionary<string, string> Items = new() {
//         { "intro", "m1_intro_01"},
//         { "silos_safe", "m1_silos_safearea_01" },
//         { "prepipes", "campaign_interlude_silo_to_pipeworks" },
//         { "prehab", "campaign_interlude_pipeworks_to_habitation_01" },
//         { "prepier", "m3_habitation_shaft_to_pier" },
//         { "pier", "m3_habitation_pier_entrance_01" },
//         { "abyss", "m3_habitation_lab_ending" },
//         // shortcuts? make failsafe? inner pier? obelisk?
//         // need to preload levels?
//     };


//     protected override Action<string[]> GetLogicCallback()
//     {
//         return args =>
//         {
//             Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabled();
//             ENT_Player player = ENT_Player.playerObject;
//             if (args.Length == 0)
//             {
//                 Accessors.CommandConsoleAccessor.EchoToConsole($"Available locations:\n-{Items.Keys.Join(delimiter: "\n-")}");
//             }
//             else if (Items.ContainsKey(args[0].ToLower()))
//             {
//                 (Items[args[0].ToLower()]);
//             }
//             else
//             {
//                 Accessors.CommandConsoleAccessor.EchoToConsole($"Invalid arguments for tp command: {args.Join()}");
//             }
//         };
//     }
// }
