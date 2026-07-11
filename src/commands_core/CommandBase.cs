using System;
using System.Linq;

namespace MoreCommands.Common;


public interface ICommand {
    string[] Aliases { get; }
    CommandTag Tag { get; }
    string Description { get; }
    bool EnablesCheatsOnUse { get; }
    Action<string[]> GetCallback();
    void Execute(string[] args);
    void OnExit();
}

public interface ITogglableCommand : ICommand {
    bool Enabled { get; set; }
}

public abstract class CommandBase : ICommand {
    public abstract string[] Aliases { get; }
    public abstract CommandTag Tag { get; }
    public abstract string Description { get; }

    public abstract bool EnablesCheatsOnUse { get; }
    public abstract void Execute(string[] args);
    public virtual void OnExit() {}

    public virtual void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteHelpers.NoArguments)
            .AutocompleteValidator(AutocompleteHelpers.ValidateNoArguments);
    }

    protected virtual bool TryValidateArguments(string[] args, out string error)
    {
        error = args.Length == 0 ? null : $"Usage: {Aliases.First()} takes no arguments";
        return args.Length == 0;
    }

    private void EnableCheatsIfNeeded()
    {
        if (EnablesCheatsOnUse) Accessors.CommandConsoleAccessor.EnsureCheatsAreEnabled();
    }

    protected bool HasRequiredContext()
    {
        switch (Tag)
        {
            case CommandTag.Player:
                if (ENT_Player.GetPlayer() != null) return true;
                Accessors.CommandConsoleAccessor.EchoToConsole("No player found");
                return false;
            case CommandTag.World:
                if (CL_GameManager.gMan != null) return true;
                Accessors.CommandConsoleAccessor.EchoToConsole("No active game world found");
                return false;
            default:
                return true;
        }
    }

    protected static Inventory GetInventoryOrWarn()
    {
        if (Inventory.instance != null) return Inventory.instance;

        Accessors.CommandConsoleAccessor.EchoToConsole("No inventory found");
        return null;
    }

    protected virtual void BeforeExecute(string[] args) {}
    protected virtual void AfterExecute() {}

    public virtual Action<string[]> GetCallback()
    {
        return args =>
        {
            if (!HasRequiredContext()) return;
            if (!TryValidateArguments(args, out string error))
            {
                Accessors.CommandConsoleAccessor.EchoToConsole(error);
                return;
            }

            BeforeExecute(args);
            EnableCheatsIfNeeded();
            Execute(args);
            AfterExecute();
            Accessors.CommandConsoleAccessor.EchoToConsole(Colors.COMMAND_SEP);
        };
    }
}

public abstract class TogglableCommandBase : CommandBase, ITogglableCommand {
    public bool Enabled { get; set; }
    private string EnabledStr => Enabled ? "enabled" : "disabled";

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteHelpers.OptionalSingleBool)
            .AutocompleteValidator(AutocompleteHelpers.ValidateOptionalSingleBool)
            .OverValue(() => Enabled);
    }

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1 && (args.Length == 0 || bool.TryParse(args[0], out _));
        error = valid ? null : $"Usage: {Aliases.First()} [true|false]";
        return valid;
    }

    protected override void BeforeExecute(string[] args)
    {
        Enabled = ParseEnabled(Enabled, args);
    }

    protected override void AfterExecute()
    {
        Accessors.CommandConsoleAccessor.EchoToConsole($"{Colors.Highlighted(Aliases.First())} {EnabledStr}");
    }

    protected static string[] WhenEnabled(bool enabled)
    {
        return [enabled ? "true" : "false"];
    }

    protected static string[] WhenDisabled(bool enabled)
    {
        return [enabled ? "false" : "true"];
    }

    private static bool ParseEnabled(bool enabled, string[] args)
    {
        return args.Length == 0 ? !enabled : bool.Parse(args[0]);
    }

    public override void OnExit()
    {
        if (!Enabled) return;

        Enabled = false;
        Execute(["false"]);
    }
}
