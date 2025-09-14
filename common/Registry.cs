using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
namespace MoreCommands.Common;

public static partial class CommandRegistry
{
    static partial void RegisterAll();
    public static List<ICommand> RegisteredCommands = [];
    public static bool Initialized = false;

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
            ICommand found = RegisteredCommands.Find(c => c.Aliases.Contains(alias));
            if (found != null)
                failedAliases.Add(alias, found.GetType().ToString());
        }
        if (failedAliases.Count > 0)
        {
            MoreCommandsPlugin.Logger.LogWarning($"Failed to register command {command.GetType()}:\n{failedAliases.Join(x => $"\t{x.Key} taken by {x.Value}", "\n")}");
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

    public static ICommand GetCommand<T>() where T : ICommand
    {
        return RegisteredCommands.FirstOrDefault(c => c.GetType() == typeof(T));
    }

    public static Action<string[]> GetCallback<T>() where T : ICommand
    {
        return RegisteredCommands.FirstOrDefault(c => c.GetType() == typeof(T))?.GetCallback() ?? (args => { MoreCommandsPlugin.Logger.LogWarning($"Command {typeof(T)} not found"); });
    }

    public static void DisableAllTogglableCommands()
    {
        foreach (var command in RegisteredCommands)
        {
            if (command is ITogglableCommand togglableCommand)
            {
                togglableCommand.Enabled = false;
            }
        }
    }
}
