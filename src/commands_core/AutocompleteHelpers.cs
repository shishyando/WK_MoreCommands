using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreCommands.Common;

public static class AutocompleteHelpers
{
    private static readonly string[] BoolValues = ["true", "false"];

    public static readonly string[] ColorValues =
    [
        "red",
        "green",
        "blue",
        "yellow",
        "cyan",
        "magenta",
        "white",
        "black",
        "grey",
        "gray",
        "orange",
    ];

    public static void NoArguments(CommandConsole.CommandAutocomplete autocomplete)
    {
        autocomplete.Reject();
    }

    public static void ValidateNoArguments(CommandConsole.CommandValidator validator)
    {
        validator.Reject();
    }

    public static void OptionalSingleFloat(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg > 0)
        {
            autocomplete.Reject();
        }
    }

    public static void ValidateOptionalSingleFloat(CommandConsole.CommandValidator validator)
    {
        if (validator.activeArg != 0
            || !float.TryParse(validator.ArgumentAt(validator.activeArg), out float value)
            || !IsFinite(value))
        {
            validator.Reject();
        }
    }

    public static void OptionalSingleBool(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg == 0)
        {
            autocomplete.FromArray(BoolValues);
            return;
        }

        autocomplete.Reject();
    }

    public static void ValidateOptionalSingleBool(CommandConsole.CommandValidator validator)
    {
        if (validator.activeArg != 0 || !bool.TryParse(validator.ArgumentAt(validator.activeArg), out _))
        {
            validator.Reject();
        }
    }

    public static void OptionalSingleFrom<T>(CommandConsole.CommandAutocomplete autocomplete, Func<Handle<T>> getHandle)
    {
        if (autocomplete.activeArg == 0)
        {
            autocomplete.FromArray(Names(getHandle));
            return;
        }

        autocomplete.Reject();
    }

    public static void ValidateOptionalSingleFilter(CommandConsole.CommandValidator validator)
    {
        if (validator.activeArg != 0)
        {
            validator.Reject();
        }
    }

    public static void ValidateOptionalSingleExactFrom<T>(CommandConsole.CommandValidator validator, Func<Handle<T>> getHandle)
    {
        if (validator.activeArg != 0 || !HasExactMatch(getHandle, validator.ArgumentAt(validator.activeArg)))
        {
            validator.Reject();
        }
    }

    public static void VariadicExactFrom<T>(CommandConsole.CommandAutocomplete autocomplete, Func<Handle<T>> getHandle)
    {
        autocomplete.FromArray(Names(getHandle));
    }

    public static void ValidateVariadicExactFrom<T>(CommandConsole.CommandValidator validator, Func<Handle<T>> getHandle)
    {
        if (!HasExactMatch(getHandle, validator.ArgumentAt(validator.activeArg)))
        {
            validator.Reject();
        }
    }

    public static string[] Names<T>(Func<Handle<T>> getHandle)
    {
        return [.. getHandle().Names().Where(x => !string.IsNullOrWhiteSpace(x))];
    }

    public static bool HasExactMatch<T>(Func<Handle<T>> getHandle, string value)
    {
        return !string.IsNullOrWhiteSpace(value)
            && getHandle().FilterExact(value).Data().Any();
    }

    public static bool IsFinite(float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }

    public static string[] WithAcceptedCurrentValue(IEnumerable<string> suggestions, string current, Func<string, bool> accepts)
    {
        string[] values = [.. suggestions.Where(value => !string.IsNullOrWhiteSpace(value))];
        if (string.IsNullOrWhiteSpace(current)
            || values.Contains(current, StringComparer.OrdinalIgnoreCase)
            || !accepts(current))
        {
            return values;
        }

        return [current, .. values];
    }
}
