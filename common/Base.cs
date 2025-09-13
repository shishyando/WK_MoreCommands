using System;

namespace MoreCommands.Common;

public static class CommandHelpers
{
    public static void UpdateEnabled(ref bool enabled, string[] args)
    {
        if (args.Length == 0)
        {
            enabled = !enabled;
        }
        else if (!bool.TryParse(args[0], out bool result))
        {
            MoreCommandsPlugin.Logger.LogInfo($"Unable to parse `{string.Join(" ", args)}`, arg needs to be a boolean (true/false/0/1).");
            return;
        }
        else
        {
            enabled = result;
        }
    }

    public static string[] WhenEnabled(bool enabled)
    {
        return [enabled.ToString().ToLower()];
    }

    public static string[] WhenDisabled(bool enabled)
    {
        return [(!enabled).ToString().ToLower()];
    }
}
