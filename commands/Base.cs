using System;

namespace MoreCommands.Commands;

public interface ICommand
{
    Action<string[]> GetCallback();
    void UpdateEnabled(string[] args);
    string[] WhenEnabled();
    string[] WhenDisabled();
    string Cmd { get; }
    CommandTag Tag { get; }
    bool Enabled { get; set; }
}


public abstract class Command<T> : ICommand where T : Command<T>
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

    public abstract string Cmd { get; }
    public abstract CommandTag Tag { get; }
    public bool Enabled { get; set; }
}
