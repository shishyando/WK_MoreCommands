using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListRegionsCommand : CommandBase
{
    public override string[] Aliases => ["listregions"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List region names, optionally filtered by substring";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1;
        error = valid ? null : "Usage: listregions [region name substring]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(autocomplete => AutocompleteHelpers.OptionalSingleFrom(autocomplete, Prefabs.Regions))
            .AutocompleteValidator(AutocompleteHelpers.ValidateOptionalSingleFilter);
    }

    public override void Execute(string[] args)
    {
        string result = Prefabs.Regions().Filter(args.FirstOrDefault() ?? "").Join();
        Accessors.CommandConsoleAccessor.EchoToConsole(
            string.IsNullOrWhiteSpace(result) ? "No matching regions" : $"- {result}");
    }

}
