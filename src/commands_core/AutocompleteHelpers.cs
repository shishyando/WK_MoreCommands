using System;
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

    public static void OptionalSingleFloat(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg > 0)
        {
            autocomplete.Reject();
        }
    }

    public static void ValidateOptionalSingleFloat(CommandConsole.CommandValidator validator)
    {
        if (validator.activeArg != 0 || !float.TryParse(validator.ArgumentAt(validator.activeArg), out _))
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

    public static void ValidateOptionalSingleFrom<T>(CommandConsole.CommandValidator validator, Func<Handle<T>> getHandle)
    {
        if (validator.activeArg != 0 || !HasMatch(getHandle, validator.ArgumentAt(validator.activeArg)))
        {
            validator.Reject();
        }
    }

    public static void VariadicFrom<T>(CommandConsole.CommandAutocomplete autocomplete, Func<Handle<T>> getHandle)
    {
        autocomplete.FromArray(Names(getHandle));
    }

    public static void ValidateVariadicFrom<T>(CommandConsole.CommandValidator validator, Func<Handle<T>> getHandle)
    {
        if (!HasMatch(getHandle, validator.ArgumentAt(validator.activeArg)))
        {
            validator.Reject();
        }
    }

    public static string[] Names<T>(Func<Handle<T>> getHandle)
    {
        return [.. getHandle().Names().Where(x => !string.IsNullOrWhiteSpace(x))];
    }

    public static bool HasMatch<T>(Func<Handle<T>> getHandle, string value)
    {
        return !string.IsNullOrWhiteSpace(value)
            && getHandle().Filter(value).Names().Any(x => !string.IsNullOrWhiteSpace(x));
    }
}
