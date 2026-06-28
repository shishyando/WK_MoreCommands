using System;
using HarmonyLib;
using MoreCommands.Common;
using UnityEngine;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(CommandConsole), "Awake")]
public static class CommandConsole_Awake_Patcher
{
    [HarmonyPostfix]
    public static void RegisterMoreCommands(CommandConsole __instance)
    {
        CommandRegistration.AddCommandsByTag(CommandTag.Console);
    }

    [HarmonyPostfix]
    public static void FixVanillaGameBug(CommandConsole __instance) // fix for non-ascii input crashing console & requiring restart
    {
        var inputFields = __instance.gameObject.GetComponentsInChildren<TMPro.TMP_InputField>(true);
        foreach (TMPro.TMP_InputField input in inputFields)
        {
            input.onValidateInput = ValidateAscii;
        }

        static char ValidateAscii(string text, int charIndex, char addedChar)
        {
            if (addedChar <= 127)
            {
                return addedChar;
            }

            return '\0';
        }
    }
}

public static class CommandRegistration
{
    public static void AddCommandsByTag(CommandTag tag)
    {
        foreach (var command in CommandRegistry.GetCommandsByTag(tag))
        {
            foreach (string alias in command.Aliases)
            {
                AddCommandIfFree(alias, command);
            }
        }
    }

    public static void AddCommandIfFree(string alias, ICommand source)
    {
        string command = alias.ToLower();

        if (CommandConsole.instance != null)
        {
            var commandsDict = Accessors.CommandConsoleAccessor.GetCommands(CommandConsole.instance);
            if (commandsDict.Contains(command))
            {
                Plugin.Beep.LogInfo($"Skipping MoreCommands alias '{command}' because an existing command already uses it");
                return;
            }
        }

        var builder = CommandConsole.BuildCommand(command, source.GetCallback())
            .NotCheat()
            .Description(source.Description);

        if (source is CommandBase commandBase)
        {
            commandBase.ConfigureBuilder(builder);
        }
    }
}

// it's important to use RegisterCommand and not AddCommand
// RegisterCommand is called from AddCommand when it ensures that instance is set
// otherwise you will get an empty instance and hence NullReferenceException
[HarmonyPatch(typeof(CommandConsole), "RegisterCommand")]
public static class CommandConsole_RegisterCommand_Patcher
{
    [HarmonyPrefix]
    public static bool DoNotOverrideExistingCommands(CommandConsole __instance, ref string command, ref object __result)
    {
        // base game registers chains commands and I don't want that behavior
        if (command == null || __instance == null) return true;

        string commandKey = command.ToLower();
        var commandsDict = Accessors.CommandConsoleAccessor.GetCommands(__instance);
        if (!commandsDict.Contains(commandKey)) return true;

        __result = commandsDict[commandKey];
        return false;
    }
}
