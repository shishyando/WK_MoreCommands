using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListItemsCommand : CommandBase
{
    public override string[] Aliases => ["listitems"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List items, filtered by `arg`";
    public override bool EnablesCheatsOnUse => false;

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(autocomplete => AutocompleteHelpers.OptionalSingleFrom(autocomplete, Prefabs.Items))
            .AutocompleteValidator(validator => AutocompleteHelpers.ValidateOptionalSingleFrom(validator, Prefabs.Items));
    }

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"- {Prefabs.Items().Filter(args.FirstOrDefault() ?? "").Join()}");
        };
    }

}
