using System;
using HarmonyLib;
using MoreCommands.Common;
using MoreCommands.Patches;

namespace MoreCommands.Commands;


public sealed class NoclipSpeed : CommandBase
{
    public override string[] Aliases => ["ns", "noclipspeed"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "set noclip speed multiplier (1 is default)";
    public override bool CheatsOnly => true;

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            if (args.Length == 0) {
                ENT_Player_Movement_Patcher.NoclipSpeedMultiplier = 1f;
            }
            else if (float.TryParse(args[0], out float m))
            {
                ENT_Player_Movement_Patcher.NoclipSpeedMultiplier = m;
            }
            else
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Invalid arguments for noclipspeed command: {args.Join(delimiter: " ")}");
            }
        };
    }
}
