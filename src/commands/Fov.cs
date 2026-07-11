using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;

public sealed class FovCommand : CommandBase
{
    public override string[] Aliases => ["fov"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "Show or set persistent player FOV from 60 to 140; reset restores 90";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1
            && (args.Length == 0
                || string.Equals(args[0], "reset", StringComparison.OrdinalIgnoreCase)
                || (float.TryParse(args[0], out float fov)
                    && AutocompleteHelpers.IsFinite(fov)
                    && fov >= 60f
                    && fov <= 140f));
        error = valid ? null : "Usage: fov [reset|60-140]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteArgs)
            .AutocompleteValidator(ValidateArgs)
            .OverValue(() => SettingsManager.settings?.playerFOV);
    }

    public override void Execute(string[] args)
    {
        if (SettingsManager.settings == null)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole("No game settings found");
            return;
        }

        if (args.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"Player FOV: {SettingsManager.settings.playerFOV:F1}");
            return;
        }

        float fov;
        if (string.Equals(args[0], "reset", StringComparison.OrdinalIgnoreCase))
        {
            fov = 90f;
        }
        else float.TryParse(args[0], out fov);

        SettingsManager.settings.playerFOV = fov;
        SettingsManager.RefreshSettings("playerFOV");
        SettingsManager.instance?.SaveSettings();
        Accessors.CommandConsoleAccessor.EchoToConsole($"Player FOV set to {fov:F1}");
    }

    private static void AutocompleteArgs(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg == 0)
        {
            autocomplete.FromArray(AutocompleteHelpers.WithAcceptedCurrentValue(
                ["reset", "90", "100", "110", "120"],
                autocomplete.ArgumentAt(0),
                value => string.Equals(value, "reset", StringComparison.OrdinalIgnoreCase)
                    || (float.TryParse(value, out float fov)
                        && AutocompleteHelpers.IsFinite(fov)
                        && fov >= 60f
                        && fov <= 140f)));
            return;
        }

        autocomplete.Reject();
    }

    private static void ValidateArgs(CommandConsole.CommandValidator validator)
    {
        string value = validator.ArgumentAt(validator.activeArg);
        if (validator.activeArg == 0
            && (string.Equals(value, "reset", StringComparison.OrdinalIgnoreCase)
                || (float.TryParse(value, out float fov)
                    && AutocompleteHelpers.IsFinite(fov)
                    && fov >= 60f
                    && fov <= 140f)))
        {
            return;
        }

        validator.Reject();
    }
}
