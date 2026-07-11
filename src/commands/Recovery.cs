using MoreCommands.Common;

namespace MoreCommands.Commands;

public sealed class HealCommand : CommandBase
{
    public override string[] Aliases => ["heal"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Heal player by amount, or to full health with no amount";
    public override bool EnablesCheatsOnUse => true;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1
            && (args.Length == 0
                || (float.TryParse(args[0], out float amount)
                    && AutocompleteHelpers.IsFinite(amount)
                    && amount >= 0f));
        error = valid ? null : "Usage: heal [non-negative amount]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteHelpers.OptionalSingleFloat)
            .AutocompleteValidator(ValidateHealAmount)
            .OverValue(() => ENT_Player.playerObject == null ? null : $"{ENT_Player.playerObject.health:F1}/{ENT_Player.playerObject.maxHealth:F1}");
    }

    public override void Execute(string[] args)
    {
        ENT_Player player = ENT_Player.playerObject;
        if (args.Length == 0)
        {
            player.health = player.maxHealth;
            Accessors.CommandConsoleAccessor.EchoToConsole($"Health: {player.health:F1}/{player.maxHealth:F1}");
            return;
        }

        float.TryParse(args[0], out float amount);
        player.Heal(amount);
        Accessors.CommandConsoleAccessor.EchoToConsole($"Health: {player.health:F1}/{player.maxHealth:F1}");
    }

    private static void ValidateHealAmount(CommandConsole.CommandValidator validator)
    {
        if (validator.activeArg != 0
            || !float.TryParse(validator.ArgumentAt(0), out float amount)
            || !AutocompleteHelpers.IsFinite(amount)
            || amount < 0f)
        {
            validator.Reject();
        }
    }
}
