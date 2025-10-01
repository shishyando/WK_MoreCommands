using System;
using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GravityCommand : CommandBase
{
    public override string[] Aliases => ["sv_gravity", "grav"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "set player gravity multiplier (1 is default)";
    public override bool CheatsOnly => true;

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabled();
            ENT_Player player = ENT_Player.playerObject;
            if (args.Length == 0) {
                player.SetGravityMult(1f);
            }
            else if (float.TryParse(args[0], out float g))
            {
                player.SetGravityMult(g);
            }
            else
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Invalid arguments for gravity command: {args.Join(delimiter: " ")}");
            }
        };
    }
}
