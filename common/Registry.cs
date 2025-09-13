using System;
using System.Collections.Generic;
using System.Linq;
namespace MoreCommands.Common;

public static partial class CommandRegistry
{
    static partial void RegisterAll();
    public static HashSet<ICommand> RegisteredCommands = [];
    public static bool Initialized = false;

    public static void InitializeCommands()
    {
        if (Initialized) return;

        RegisterAll();

        Initialized = true;
    }

    public static void Register(ICommand command)
    {
        RegisteredCommands.Add(command);
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

    public static void DisableAllCommands()
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
