using HarmonyLib;
using MoreCommands.Common;
using MoreCommands.Patches;

namespace MoreCommands.Commands;


public sealed class NoclipSpeed : CommandBase
{
    public override string[] Aliases => ["ns", "noclipspeed"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Set a non-negative noclip speed multiplier; no argument resets it to 1";
    public override bool EnablesCheatsOnUse => true;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1
            && (args.Length == 0
                || (float.TryParse(args[0], out float value)
                    && AutocompleteHelpers.IsFinite(value)
                    && value >= 0f));
        error = valid ? null : "Usage: noclipspeed [non-negative finite multiplier]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteHelpers.OptionalSingleFloat)
            .AutocompleteValidator(ValidateMultiplier)
            .OverValue(() => ENT_Player_Movement_Patcher.NoclipSpeedMultiplier);
    }

    public override void Execute(string[] args)
    {
        if (args.Length == 0) {
            ENT_Player_Movement_Patcher.NoclipSpeedMultiplier = 1f;
            Accessors.CommandConsoleAccessor.EchoToConsole("Noclip speed set to default");
        }
        else
        {
            float.TryParse(args[0], out float m);
            ENT_Player_Movement_Patcher.NoclipSpeedMultiplier = m;
            Accessors.CommandConsoleAccessor.EchoToConsole($"Noclip speed set to {m:0.###}");
        }
    }

    public override void OnExit()
    {
        ENT_Player_Movement_Patcher.NoclipSpeedMultiplier = 1f;
    }

    private static void ValidateMultiplier(CommandConsole.CommandValidator validator)
    {
        if (validator.activeArg != 0
            || !float.TryParse(validator.ArgumentAt(0), out float value)
            || !AutocompleteHelpers.IsFinite(value)
            || value < 0f)
        {
            validator.Reject();
        }
    }
}
