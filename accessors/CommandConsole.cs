using System;
using HarmonyLib;

namespace MoreCommands.Accessors;

public static class CommandConsoleAccessor {
    private static readonly Action<CommandConsole, string[]> EnableCheatsRaw =
    AccessTools.MethodDelegate<Action<CommandConsole, string[]>>(
        AccessTools.Method(typeof(CommandConsole), "EnableCheatsCommand", new[] { typeof(string[]) })
    );

    private static readonly Action<CommandConsole> CheatsEnabler =
        inst => EnableCheatsRaw(inst, Array.Empty<string>());

    public static void EnableCheats() {
        CommandConsole inst = CommandConsole.instance;
        if (inst == null) {
            MoreCommandsPlugin.Logger.LogWarning("CommandConsoleAccessor::EnableCheats instance is null");
            return;
        }
        CheatsEnabler(CommandConsole.instance);
    }
}
