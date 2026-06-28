using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListEntitiesCommand : CommandBase
{
    public override string[] Aliases => ["listentities"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List entities, filtered by `arg`";
    public override bool EnablesCheatsOnUse => false;

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(autocomplete => AutocompleteHelpers.OptionalSingleFrom(autocomplete, Prefabs.Entities))
            .AutocompleteValidator(validator => AutocompleteHelpers.ValidateOptionalSingleFrom(validator, Prefabs.Entities));
    }

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"- {Prefabs.Entities().Filter(args.FirstOrDefault() ?? "").Join()}");
        };
    }

}
