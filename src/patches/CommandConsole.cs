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
        foreach (var c in CommandRegistry.GetCommandsByTag(CommandTag.Console))
        {
            foreach (var alias in c.Aliases)
            {
                CommandConsole.RemoveCommand(alias); // if game already registered some commands, I will override them
                CommandConsole.AddCommand(alias, c.GetCallback(), false);
            }
        }
        foreach (var c in CommandRegistry.GetCommandsByTag(CommandTag.World))
        {
            foreach (var alias in c.Aliases)
            {
                CommandConsole.RemoveCommand(alias); // if game already registered some commands, I will override them
                CommandConsole.AddCommand(alias, c.GetCallback(), false);
            }
        }
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

// it's important to use RegisterCommand and not AddCommand
// RegisterCommand is called from AddCommand when it ensures that instance is set
// otherwise you will get an empty instance and hence NullReferenceException
[HarmonyPatch(typeof(CommandConsole), "RegisterCommand")]
public static class CommandConsole_RegisterCommand_Patcher
{
    [HarmonyPrefix]
    public static bool DoNotOverrideExistingCommands(CommandConsole __instance, ref string command)
    {
        // base game registers chains commands and I don't want that behavior
        if (command == null || CommandConsole.instance == null || __instance == null) return false;
        var commandsDict = Accessors.CommandConsoleAccessor.commandsRef(CommandConsole.instance);
        return !commandsDict.ContainsKey(command);
    }
}
