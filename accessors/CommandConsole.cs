using System;
using HarmonyLib;

namespace MoreCommands.Accessors;

public static class CommandConsoleAccessor {
    private static readonly Action<CommandConsole, string[]> EnableCheatsRaw =
    AccessTools.MethodDelegate<Action<CommandConsole, string[]>>(
        AccessTools.Method(typeof(CommandConsole), "EnableCheatsCommand", [typeof(string[])])
    );

    private static readonly Action<CommandConsole> CheatsEnabler =
        inst => EnableCheatsRaw(inst, ["true"]);

    public static void EnsureCheatsAreEnabled() {
        if (CommandConsole.hasCheated)
        {
            return;
        }
        CommandConsole inst = CommandConsole.instance;
        if (inst == null)
        {
            MoreCommandsPlugin.Logger.LogWarning("CommandConsoleAccessor::EnableCheats instance is null");
            return;
        }
        CheatsEnabler(CommandConsole.instance);
    }
}
