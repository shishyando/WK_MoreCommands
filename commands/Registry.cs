
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreCommands.Commands;

public static class CommandRegistry
{
    private static readonly List<ICommand> _registeredCommands = [];
    private static bool _initialized = false;

    public static void InitializeCommands()
    {
        if (_initialized) return;
        
        var assembly = typeof(CommandRegistry).Assembly;
        var commandTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.ContainsGenericParameters && InheritsFromGenericCommand(t))
            .ToList();

        foreach (var c in commandTypes) {
            try
            {
                var commandInstance = (ICommand)Activator.CreateInstance(c);
                Register(commandInstance);
                MoreCommandsPlugin.Logger.LogInfo($"Registered command: {commandInstance.Cmd}");
            }
            catch (Exception ex)
            {
                MoreCommandsPlugin.Logger.LogError($"Failed to register command {c.FullName}: {ex.Message}");
            }
        }
        
        _initialized = true;
    }

    private static bool InheritsFromGenericCommand(Type type)
    {
        for (var cur = type; cur != null && cur != typeof(object); cur = cur.BaseType)
        {
            var def = cur.IsGenericType ? cur.GetGenericTypeDefinition() : cur;
            if (def == typeof(Command<>))
            {
                return true;
            }
        }
        return false;
    }

    public static void Register(ICommand command)
    {
        if (!_registeredCommands.Any(c => c.GetType() == command.GetType()))
        {
            _registeredCommands.Add(command);
        }
    }

    public static List<ICommand> GetCommandsByTag(CommandTag tag)
    {
        var result = _registeredCommands.Where(c => c.Tag == tag).ToList();
        return result;
    }

    public static List<ICommand> GetAllCommands()
    {
        return [.. _registeredCommands];
    }

    public static ICommand GetCommandByName(string cmdName)
    {
        return _registeredCommands.FirstOrDefault(c => c.Cmd.Equals(cmdName, StringComparison.OrdinalIgnoreCase));
    }
}
