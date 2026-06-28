using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ManCommand : CommandBase
{
    public override string[] Aliases => ["man", "mhelp", "morecommandshelp"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "prints MoreCommands with their descriptions ('+' = enables cheats)";
    public override bool EnablesCheatsOnUse => false;

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
            autocomplete.FromArray([.. CommandRegistry.GetAllCommands().SelectMany(command => command.Aliases)]);
            return;
        }

        autocomplete.Reject();
    }

    private static void ValidateCommand(CommandConsole.CommandValidator validator)
    {
        string value = validator.ArgumentAt(validator.activeArg);
        if (validator.activeArg != 0 || !CommandRegistry.GetAllCommands().Any(command => command.Aliases.Contains(value)))
        {
            validator.Reject();
        }
    }

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            foreach (ICommand c in CommandRegistry.GetAllCommands().OrderBy(x => (x.Tag, x.Aliases[0])))
            {
                if (args.Length == 0 || c.Aliases.Contains(args[0]))
                {
                    Accessors.CommandConsoleAccessor.EchoToConsole(Colors.FormatCommand(c));
                }
            }
        };
    }
}
