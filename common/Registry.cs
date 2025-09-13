
using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine.UIElements.Collections;

namespace MoreCommands.Common;

public static class CommandRegistry
{
    public static Dictionary<string, ICommand> RegisteredCommands = [];
    public static bool Initialized = false;

    public static void InitializeCommands()
    {
        if (Initialized) return;

        var assembly = typeof(CommandRegistry).Assembly;
        var commandTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.ContainsGenericParameters && DerivedFrom(t, [typeof(TogglableCommand<>), typeof(OneshotCommand<>)]))
            .ToList();

        foreach (var c in commandTypes)
        {
            try
            {
                var commandInstance = (ICommand)Activator.CreateInstance(c);
                Register(commandInstance);
                MoreCommandsPlugin.Logger.LogInfo($"Registered command: {c.Name} as {commandInstance.Aliases.Join()}");
            }
            catch (Exception ex)
            {
                MoreCommandsPlugin.Logger.LogError($"Failed to register command {c.Name}: {ex.Message}");
            }
        }

        Initialized = true;
    }

    private static bool DerivedFrom(Type type, Type[] possibleParents) {
        for (var cur = type; cur != null && cur != typeof(object); cur = cur.BaseType)
        {
            var def = cur.IsGenericType ? cur.GetGenericTypeDefinition() : cur;
            if (possibleParents.Contains(def))
            {
                return true;
            }
        }
        return false;
    }

    public static void Register(ICommand command)
    {
        foreach (string alias in command.Aliases)
        {
            if (RegisteredCommands.ContainsKey(alias))
            {
                MoreCommandsPlugin.Logger.LogWarning($"Command {command.GetType()} tried to register as {alias}, but it's occupied by {RegisteredCommands.Get(alias).GetType()}, skipping...");
                continue;
            }
            RegisteredCommands.Add(alias, command);
        }
    }

    public static List<ICommand> GetCommandsByTag(CommandTag tag)
    {
        return [.. RegisteredCommands.Values.Where(c => c.Tag == tag)];
    }

    public static List<KeyValuePair<string, ICommand>> GetAllCommands()
    {
        return [.. RegisteredCommands];
    }

    public static ICommand GetCommandByName(string cmdName)
    {
        return RegisteredCommands.Get(cmdName);
    }

    public static void DisableAllCommands()
    {
        foreach (ICommand command in RegisteredCommands.Values)
        {
            if (command is ITogglableCommand toggleableCommand)
            {
                toggleableCommand.Enabled = false;
            }
        }
    }
}
