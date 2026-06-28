using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListRegionsCommand : CommandBase
{
    public override string[] Aliases => ["listregions"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List regions, filtered by `arg`";
    public override bool EnablesCheatsOnUse => false;

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(autocomplete => AutocompleteHelpers.OptionalSingleFrom(autocomplete, Prefabs.Regions))
            .AutocompleteValidator(validator => AutocompleteHelpers.ValidateOptionalSingleFrom(validator, Prefabs.Regions));
    }

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"- {Prefabs.Regions().Filter(args.FirstOrDefault() ?? "").Join()}");
        };
    }

}
