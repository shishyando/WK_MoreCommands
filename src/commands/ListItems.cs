using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListItemsCommand : CommandBase
{
    public override string[] Aliases => ["listitems"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List item IDs, optionally filtered by substring";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1;
        error = valid ? null : "Usage: listitems [item ID substring]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(autocomplete => AutocompleteHelpers.OptionalSingleFrom(autocomplete, Prefabs.Items))
            .AutocompleteValidator(AutocompleteHelpers.ValidateOptionalSingleFilter);
    }

    public override void Execute(string[] args)
    {
        string result = Prefabs.Items().Filter(args.FirstOrDefault() ?? "").Join();
        Accessors.CommandConsoleAccessor.EchoToConsole(
            string.IsNullOrWhiteSpace(result) ? "No matching items" : $"- {result}");
    }

}
