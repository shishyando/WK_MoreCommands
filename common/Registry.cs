using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MoreCommands.Common;

public static partial class CommandRegistry
{
    static partial void RegisterAll();
    public sealed class CommandDescriptor
    {
        public string[] Aliases { get; set; }
        public CommandTag Tag { get; set; }
        public Action<string[]> Callback { get; set; }
        public Type DeclaringType { get; set; }
        public string Description { get; set; }

        public Action<string[]> GetCallback() => Callback;
    }

    public static Dictionary<string, CommandDescriptor> RegisteredCommands = [];
    public static bool Initialized = false;

    public static void InitializeCommands()
    {
        if (Initialized) return;

        RegisterAll();

        Initialized = true;
    }

    public static void Register(CommandDescriptor command)
    {
        foreach (string alias in command.Aliases)
        {
            if (RegisteredCommands.ContainsKey(alias))
            {
                MoreCommandsPlugin.Logger.LogWarning($"Command {command.DeclaringType} tried to register as {alias}, but it's occupied by {RegisteredCommands[alias].DeclaringType}, skipping...");
                continue;
            }
            RegisteredCommands.Add(alias, command);
        }
    }

    public static List<CommandDescriptor> GetCommandsByTag(CommandTag tag)
    {
        return [.. RegisteredCommands.Values.Where(c => c.Tag == tag).Distinct()];
    }

    public static List<KeyValuePair<string, CommandDescriptor>> GetAllCommands()
    {
        return [.. RegisteredCommands];
    }

    public static CommandDescriptor GetCommandByName(string cmdName)
    {
        return RegisteredCommands.TryGetValue(cmdName, out var cmd) ? cmd : null;
    }

    public static void DisableAllCommands()
    {
        foreach (var type in RegisteredCommands.Values.Select(v => v.DeclaringType).Distinct())
        {
            try
            {
                var prop = type.GetProperty("Enabled", BindingFlags.Public | BindingFlags.Static);
                if (prop != null && prop.PropertyType == typeof(bool) && prop.CanWrite)
                {
                    prop.SetValue(null, false);
                    continue;
                }
                var field = type.GetField("Enabled", BindingFlags.Public | BindingFlags.Static);
                if (field != null && field.FieldType == typeof(bool))
                {
                    field.SetValue(null, false);
                }
            }
            catch (Exception ex)
            {
                MoreCommandsPlugin.Logger.LogWarning($"Failed to disable command {type.Name}: {ex.Message}");
            }
        }
    }
}
