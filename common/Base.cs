using System;
using MoreCommands.Commands;

namespace MoreCommands.Common;

public interface ICommand
{
    Action<string[]> GetCallback();
    string[] Aliases { get; }
    CommandTag Tag { get; }
}

public interface ITogglableCommand : ICommand
{
    bool Enabled { get; set; }
}

public abstract class TogglableCommand<T> : ITogglableCommand where T : TogglableCommand<T>
{
    public abstract Action<string[]> GetCallback();

    public void UpdateEnabled(string[] args)
    {
        if (args.Length == 0)
        {
            Enabled = !Enabled;
        }
        else if (!bool.TryParse(args[0], out bool result))
        {
            MoreCommandsPlugin.Logger.LogInfo($"Unable to parse `{string.Join(" ", args)}`, arg needs to be a boolean (true/false/0/1).");
            return;
        }
        else
        {
            Enabled = result;
        }
    }

    public string[] WhenEnabled()
    {
        return [Enabled.ToString().ToLower()];
    }
    public string[] WhenDisabled()
    {
        return [(!Enabled).ToString().ToLower()];
    }

    public abstract string[] Aliases { get; }
    public abstract CommandTag Tag { get; }
    public bool Enabled { get; set; }
}

public abstract class OneshotCommand<T> : ICommand where T : OneshotCommand<T>
{
    public abstract Action<string[]> GetCallback();

    public abstract string[] Aliases { get; }
    public abstract CommandTag Tag { get; }
}
