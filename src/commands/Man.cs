using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ManCommand : CommandBase
{
    public override string[] Aliases => ["man", "mhelp", "morecommandshelp"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List MoreCommands help, optionally for one command; '+' means it enables cheats";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1
            && (args.Length == 0
                || CommandRegistry.GetAllCommands().Any(command =>
                    command.Aliases.Contains(args[0], StringComparer.OrdinalIgnoreCase)));
        error = valid ? null : "Usage: man [MoreCommands command]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteCommand)
            .AutocompleteValidator(ValidateCommand);
    }

    private static void AutocompleteCommand(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg == 0)
        {
            string[] aliases = [.. CommandRegistry.GetAllCommands().SelectMany(command => command.Aliases)];
            autocomplete.FromArray(aliases);
            return;
        }

        autocomplete.Reject();
    }

    private static void ValidateCommand(CommandConsole.CommandValidator validator)
    {
        string value = validator.ArgumentAt(validator.activeArg);
        if (validator.activeArg != 0
            || !CommandRegistry.GetAllCommands().Any(command =>
                command.Aliases.Contains(value, StringComparer.OrdinalIgnoreCase)))
        {
            validator.Reject();
        }
    }

    public override void Execute(string[] args)
    {
        foreach (ICommand c in CommandRegistry.GetAllCommands().OrderBy(x => (x.Tag, x.Aliases[0])))
        {
            if (args.Length == 0 || c.Aliases.Contains(args[0], StringComparer.OrdinalIgnoreCase))
            {
                Accessors.CommandConsoleAccessor.EchoToConsole(Colors.FormatCommand(c));
            }
        }
    }
}
