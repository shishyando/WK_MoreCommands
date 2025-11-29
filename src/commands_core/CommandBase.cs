using System;
using System.Linq;

namespace MoreCommands.Common;


public interface ICommand {
    string[] Aliases { get; }
    CommandTag Tag { get; }
    string Description { get; }
    bool CheatsOnly { get; }
    Action<string[]> GetCallback(bool withSuffix = true);
    Action<string[]> GetLogicCallback();
    void OnExit();
}

public interface ITogglableCommand : ICommand {
    bool Enabled { get; set; }
    void UpdateEnabled(string[] args);
}

public abstract class CommandBase : ICommand {
    public abstract string[] Aliases { get; }
    public abstract CommandTag Tag { get; }
    public abstract string Description { get; }

    public abstract bool CheatsOnly { get; }
    public abstract Action<string[]> GetLogicCallback();
    public virtual void OnExit() {}

    public void EnsureCheats(string[] args)
    {
        if (CheatsOnly) Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabled();
    }

    public void PrintSuffix(string[] args)
    {
        Accessors.CommandConsoleAccessor.EchoToConsole(Colors.COMMAND_SEP);
    }

    public virtual Action<string[]> GetCallback(bool withSuffix = true)
    {
        return EnsureCheats + GetLogicCallback() + (withSuffix ? PrintSuffix : args => {});
    }
}

public abstract class TogglableCommandBase : CommandBase, ITogglableCommand {
    public bool Enabled { get; set; }
    public string EnabledStr => Enabled ? "enabled" : "disabled";

    public void UpdateEnabled(string[] args)
    {
        Enabled = ParseEnabled(Enabled, args);
    }

    public void PrintEnabled(string[] args)
    {
        Accessors.CommandConsoleAccessor.EchoToConsole($"{Colors.Highlighted(Aliases.First())} {EnabledStr}");
    }

    public sealed override Action<string[]> GetCallback(bool withSuffix = true)
    {
        return UpdateEnabled + (Action<string[]>)EnsureCheats + GetLogicCallback() + PrintEnabled + (withSuffix ? PrintSuffix : args => {});
    }

    public static string[] WhenEnabled(bool enabled)
    {
        return [enabled.ToString().ToLower()];
    }

    public static string[] WhenDisabled(bool enabled)
    {
        return [(!enabled).ToString().ToLower()];
    }

    public static bool ParseEnabled(bool enabled, string[] args)
    {
        if (args.Length == 0 || !bool.TryParse(args[0], out bool result))
        {
            return !enabled;
        }
        return result;
    }

    public override void OnExit()
    {
        GetCallback(withSuffix: false)(["false"]);
    }
}
