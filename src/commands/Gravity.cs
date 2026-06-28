using System;
using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GravityCommand : CommandBase
{
    private static readonly AccessTools.FieldRef<ENT_Player, float> GravityMultRef = AccessTools.FieldRefAccess<ENT_Player, float>("gravityMult");

    public override string[] Aliases => ["sv_gravity", "grav"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "set player gravity multiplier (1 is default)";
    public override bool EnablesCheatsOnUse => true;

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteHelpers.OptionalSingleFloat)
            .AutocompleteValidator(AutocompleteHelpers.ValidateOptionalSingleFloat)
            .OverValue(CurrentGravityMultiplier);
    }

    private static object CurrentGravityMultiplier()
    {
        ENT_Player player = ENT_Player.playerObject;
        return player == null ? null : GravityMultRef(player);
    }

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            ENT_Player player = ENT_Player.playerObject;
            if (player == null)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole("No player found");
                return;
            }

            if (args.Length == 0) {
                player.SetGravityMult(1f);
                Accessors.CommandConsoleAccessor.EchoToConsole($"Player gravity set to 1.0");
            }
            else if (float.TryParse(args[0], out float g))
            {
                player.SetGravityMult(g);
                Accessors.CommandConsoleAccessor.EchoToConsole($"Player gravity set to {g:F1}");
            }
            else
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Invalid arguments for gravity command: {args.Join(delimiter: " ")}");
            }
        };
    }

    public override void OnExit()
    {
        ENT_Player player = ENT_Player.playerObject;
        player?.SetGravityMult(1f);
    }
}
