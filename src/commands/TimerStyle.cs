using System;
using System.Linq;
using HarmonyLib;
using MoreCommands.Common;
using TMPro;
using UnityEngine;

namespace MoreCommands.Commands;

public sealed class TimerStyleCommand : CommandBase
{
    private static readonly string[] Subcommands = ["preset", "color", "outline", "size", "spacing", "alpha", "style", "reset"];

    public override string[] Aliases => ["timerstyle"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "Show or configure persistent timer presets, colors, outline, size, spacing, alpha, and font style";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length == 0;
        if (args.Length > 0)
        {
            string command = args[0].ToLowerInvariant();
            valid = command switch
            {
                "reset" => args.Length == 1,
                "preset" => args.Length == 2 && TimerStyleController.HasPreset(args[1]),
                "color" => args.Length == 2 && ColorUtility.TryParseHtmlString(args[1], out _),
                "outline" => (args.Length == 2 || args.Length == 3)
                    && ColorUtility.TryParseHtmlString(args[1], out _)
                    && (args.Length == 2
                        || (float.TryParse(args[2], out float width)
                            && AutocompleteHelpers.IsFinite(width)
                            && width >= 0f
                            && width <= 1f)),
                "size" => args.Length == 2 && IsFiniteInRange(args[1], 8f, 160f),
                "spacing" => args.Length == 2 && IsFiniteInRange(args[1], -50f, 100f),
                "alpha" => args.Length == 2 && IsFiniteInRange(args[1], 0f, 1f),
                "style" => args.Length == 2 && TimerStyleController.TryParseFontStyle(args[1], out _),
                _ => false,
            };
        }

        error = valid
            ? null
            : "Usage: timerstyle [reset|preset|color|outline|size|spacing|alpha|style] [value]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteArgs)
            .AutocompleteValidator(ValidateArgs)
            .OverValue(TimerStyleController.Describe);
    }

    public override void Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"{TimerStyleController.Describe()}\nPresets: {TimerStyleController.PresetNames().Join()}");
            return;
        }

        string cmd = args[0].ToLowerInvariant();
        switch (cmd)
        {
            case "reset":
                TimerStyleController.Reset();
                break;
            case "preset":
                TimerStyleController.SetPreset(args[1]);
                break;
            case "color":
                ColorUtility.TryParseHtmlString(args[1], out Color faceColor);
                TimerStyleController.SetFaceColor(faceColor);
                break;
            case "outline":
                ColorUtility.TryParseHtmlString(args[1], out Color outlineColor);
                float? width = null;
                if (args.Length > 2)
                {
                    float.TryParse(args[2], out float parsedWidth);
                    width = parsedWidth;
                }
                TimerStyleController.SetOutline(outlineColor, width);
                break;
            case "size":
                float.TryParse(args[1], out float size);
                TimerStyleController.SetFontSize(size);
                break;
            case "spacing":
                float.TryParse(args[1], out float spacing);
                TimerStyleController.SetCharacterSpacing(spacing);
                break;
            case "alpha":
                float.TryParse(args[1], out float alpha);
                TimerStyleController.SetAlpha(alpha);
                break;
            case "style":
                TimerStyleController.TryParseFontStyle(args[1], out FontStyles fontStyle);
                TimerStyleController.SetFontStyle(fontStyle);
                break;
        }

        Accessors.CommandConsoleAccessor.EchoToConsole($"Timer style: {TimerStyleController.Describe()}");
    }

    private static void AutocompleteArgs(CommandConsole.CommandAutocomplete autocomplete)
    {
        switch (autocomplete.activeArg)
        {
            case 0:
                autocomplete.FromArray(Subcommands);
                return;
            case 1:
                switch (autocomplete.ArgumentAt(0).ToLowerInvariant())
                {
                    case "preset":
                        autocomplete.FromArray(TimerStyleController.PresetNames());
                        return;
                    case "color":
                    case "outline":
                        autocomplete.FromArray(AutocompleteHelpers.WithAcceptedCurrentValue(
                            AutocompleteHelpers.ColorValues,
                            autocomplete.ArgumentAt(1),
                            value => ColorUtility.TryParseHtmlString(value, out _)));
                        return;
                    case "style":
                        autocomplete.FromArray(TimerStyleController.StyleNames);
                        return;
                    default:
                        return;
                }
            case 2:
                if (string.Equals(autocomplete.ArgumentAt(0), "outline", StringComparison.OrdinalIgnoreCase)) return;
                break;
        }

        autocomplete.Reject();
    }

    private static void ValidateArgs(CommandConsole.CommandValidator validator)
    {
        string subcommand = validator.ArgumentAt(0).ToLowerInvariant();
        string value = validator.ArgumentAt(validator.activeArg);

        if (validator.activeArg == 0)
        {
            if (Subcommands.Contains(value, StringComparer.OrdinalIgnoreCase)) return;
        }
        else if (validator.activeArg == 1)
        {
            switch (subcommand)
            {
                case "preset":
                    if (TimerStyleController.HasPreset(value)) return;
                    break;
                case "color":
                case "outline":
                    if (ColorUtility.TryParseHtmlString(value, out _)) return;
                    break;
                case "size":
                    if (IsFiniteInRange(value, 8f, 160f)) return;
                    break;
                case "spacing":
                    if (IsFiniteInRange(value, -50f, 100f)) return;
                    break;
                case "alpha":
                    if (IsFiniteInRange(value, 0f, 1f)) return;
                    break;
                case "style":
                    if (TimerStyleController.TryParseFontStyle(value, out _)) return;
                    break;
            }
        }
        else if (validator.activeArg == 2 && subcommand == "outline")
        {
            if (IsFiniteInRange(value, 0f, 1f)) return;
        }

        validator.Reject();
    }

    private static bool IsFiniteInRange(string value, float min, float max)
    {
        return float.TryParse(value, out float parsed)
            && AutocompleteHelpers.IsFinite(parsed)
            && parsed >= min
            && parsed <= max;
    }
}
