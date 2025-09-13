using System;

namespace MoreCommands.Common;


public interface ICommand {
    string[] Aliases { get; }
    CommandTag Tag { get; }
    string Description { get; }
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
    protected abstract Action<string[]> GetLogicCallback();

    public virtual Action<string[]> GetCallback()
    {
        return GetLogicCallback();
    }
}

public abstract class TogglableCommandBase : CommandBase, ITogglableCommand {
    public bool Enabled { get; set; }

    public void UpdateEnabled(string[] args)
    {
        Enabled = ArgParse.ParseEnabled(Enabled, args);
    }

    public void UpdateEnabledAndEnsureCheats(string[] args)
    {
        UpdateEnabled(args);
        if (Enabled)
        {
            Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabled();
        }
    }

    public sealed override Action<string[]> GetCallback()
    {
        return UpdateEnabledAndEnsureCheats + GetLogicCallback();
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
