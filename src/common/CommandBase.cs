using System;
using UnityEngine.PlayerLoop;

namespace MoreCommands.Common;


public interface ICommand {
    string[] Aliases { get; }
    CommandTag Tag { get; }
    string Description { get; }
    bool CheatsOnly { get; }
    Action<string[]> GetCallback();
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
    protected abstract Action<string[]> GetLogicCallback();

    public void EnsureCheats(string[] args)
    {
        if (CheatsOnly) Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabled();
    }

    public void PrintSuffix(string[] args)
    {
        Accessors.CommandConsoleAccessor.EchoToConsole($"<color=grey>---------------------</color>");
    }

    public virtual Action<string[]> GetCallback()
    {
        return EnsureCheats + GetLogicCallback() + PrintSuffix;
    }
}

public abstract class TogglableCommandBase : CommandBase, ITogglableCommand {
    public bool Enabled { get; set; }
    

    public void UpdateEnabled(string[] args)
    {
        Enabled = ArgParse.ParseEnabled(Enabled, args);
    }

    public sealed override Action<string[]> GetCallback()
    {
        return UpdateEnabled + (Action<string[]>)EnsureCheats + GetLogicCallback();
    }

    public static string[] WhenEnabled(bool enabled)
    {
        return [enabled.ToString().ToLower()];
    }

    public static string[] WhenDisabled(bool enabled)
    {
        return [(!enabled).ToString().ToLower()];
    }
}
