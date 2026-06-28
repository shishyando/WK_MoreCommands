using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListSubregionsCommand : CommandBase
{
    public override string[] Aliases => ["listsubregions"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List subregions, filtered by `arg`";
    public override bool EnablesCheatsOnUse => false;

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(autocomplete => AutocompleteHelpers.OptionalSingleFrom(autocomplete, Prefabs.Subregions))
            .AutocompleteValidator(validator => AutocompleteHelpers.ValidateOptionalSingleFrom(validator, Prefabs.Subregions));
    }

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Accessors.CommandConsoleAccessor.EchoToConsole($"- {Prefabs.Subregions().Filter(args.FirstOrDefault() ?? "").Join()}");
        };
    }

}
