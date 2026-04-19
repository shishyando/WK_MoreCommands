using HarmonyLib;
using System;
using System.Collections;
using System.Reflection;

namespace MoreCommands.Accessors;

public static class CommandConsoleAccessor
{
    public static IDictionary GetCommands(CommandConsole instance)
    {
        FieldInfo commandsField = AccessTools.Field(typeof(CommandConsole), "states");

        object commandStackInstance = commandsField.GetValue(instance);
        object currentState = commandsField.FieldType.GetMethod("Peek").Invoke(commandStackInstance, null);

        FieldInfo commands = AccessTools.Field(AccessTools.Inner(typeof(CommandConsole), "CommandLineState"), "commands");

        return (IDictionary)commands.GetValue(currentState);
    }

    private static readonly Action<CommandConsole, string[]> EnableCheatsRaw =
    AccessTools.MethodDelegate<Action<CommandConsole, string[]>>(
        AccessTools.Method(typeof(CommandConsole), "EnableCheatsCommand", [typeof(string[])])
    );

    private static readonly Action<CommandConsole, string> AddMessageToHistory =
    AccessTools.MethodDelegate<Action<CommandConsole, string>>(
        AccessTools.Method(typeof(CommandConsole), "AddMessageToHistory", [typeof(string)])
    );

    private static readonly Action<CommandConsole> CheatsEnabler =
        inst => EnableCheatsRaw(inst, ["true"]);

    public static void EnsureCheatsAreEnabled()
    {
        if (CommandConsole.hasCheated)
        {
            return;
        }
        CommandConsole inst = CommandConsole.instance;
        if (inst == null)
        {
            Plugin.Beep.LogWarning("CommandConsoleAccessor::EnableCheats instance is null");
            return;
        }
        CheatsEnabler(CommandConsole.instance);
    }

    public static void EchoToConsole(string msg)
    {
        CommandConsole inst = CommandConsole.instance;
        if (inst == null)
        {
            Plugin.Beep.LogWarning("CommandConsoleAccessor::EnableCheats instance is null");
            return;
        }
        AddMessageToHistory(inst, msg);
    }

}
