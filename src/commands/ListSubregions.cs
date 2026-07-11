using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListSubregionsCommand : CommandBase
{
    public override string[] Aliases => ["listsubregions"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List subregion names, optionally filtered by substring";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1;
        error = valid ? null : "Usage: listsubregions [subregion name substring]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(autocomplete => AutocompleteHelpers.OptionalSingleFrom(autocomplete, Prefabs.Subregions))
            .AutocompleteValidator(AutocompleteHelpers.ValidateOptionalSingleFilter);
    }

    public override void Execute(string[] args)
    {
        string result = Prefabs.Subregions().Filter(args.FirstOrDefault() ?? "").Join();
        Accessors.CommandConsoleAccessor.EchoToConsole(
            string.IsNullOrWhiteSpace(result) ? "No matching subregions" : $"- {result}");
    }

}
