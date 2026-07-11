using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace MoreCommands.Common;

public static partial class CommandRegistry
{
    static partial void RegisterAll();
    private static readonly List<ICommand> RegisteredCommands = [];
    private static bool Initialized;

    public static void InitializeCommands()
    {
        if (Initialized) return;

        RegisterAll();

        Initialized = true;
    }

    public static void Register(ICommand command) // check aliases (external commands can conflict)
    {
        Dictionary<string, string> failedAliases = [];
        foreach (string alias in command.Aliases)
        {
            ICommand found = RegisteredCommands.Find(c => c.Aliases.Contains(alias, StringComparer.OrdinalIgnoreCase));
            if (found != null)
                failedAliases.TryAdd(alias, found.GetType().ToString());
        }
        if (failedAliases.Count > 0)
        {
            Plugin.Beep.LogWarning($"Failed to register command {command.GetType()}:\n{failedAliases.Join(x => $"\t{x.Key} taken by {x.Value}", "\n")}");
        }
        else
        {
            // aliases are readonly, so I don't want to bother to register only a free subset of them
            RegisteredCommands.Add(command);
        }
    }

    public static List<ICommand> GetAllCommands()
    {
        return [.. RegisteredCommands.OrderBy(c => c.Aliases[0])];
    }

    public static List<ICommand> GetCommandsByTag(CommandTag tag)
    {
        return [.. RegisteredCommands.Where(c => c.Tag == tag)];
    }

    public static T GetCommand<T>() where T : class, ICommand
    {
        return RegisteredCommands.OfType<T>().FirstOrDefault();
    }

    public static void SetTogglable<T>(bool enabled) where T : class, ICommand
    {
        if (GetCommand<T>() is not ITogglableCommand command)
        {
            Plugin.Beep.LogWarning($"Togglable command {typeof(T)} not found");
            return;
        }

        command.Enabled = enabled;
        command.Execute([enabled ? "true" : "false"]);
    }

    public static void OnExit()
    {
        foreach (var command in RegisteredCommands)
        {
            command.OnExit();
        }
    }
}
