using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GravityCommand : CommandBase
{
    private static readonly AccessTools.FieldRef<ENT_Player, float> GravityMultRef = AccessTools.FieldRefAccess<ENT_Player, float>("gravityMult");

    public override string[] Aliases => ["sv_gravity", "grav"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Set the player gravity multiplier; no argument resets it to 1";
    public override bool EnablesCheatsOnUse => true;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1
            && (args.Length == 0
                || (float.TryParse(args[0], out float value) && AutocompleteHelpers.IsFinite(value)));
        error = valid ? null : "Usage: grav [finite multiplier]";
        return valid;
    }

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

    public override void Execute(string[] args)
    {
        ENT_Player player = ENT_Player.playerObject;
        if (player == null)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole("No player found");
            return;
        }

        if (args.Length == 0) {
            player.SetGravityMult(1f);
            Accessors.CommandConsoleAccessor.EchoToConsole("Player gravity set to 1");
        }
        else
        {
            float.TryParse(args[0], out float g);
            player.SetGravityMult(g);
            Accessors.CommandConsoleAccessor.EchoToConsole($"Player gravity set to {g:0.###}");
        }
    }

    public override void OnExit()
    {
        ENT_Player player = ENT_Player.playerObject;
        player?.SetGravityMult(1f);
    }
}
