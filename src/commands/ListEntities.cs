using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ListEntitiesCommand : CommandBase
{
    public override string[] Aliases => ["listentities"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "List entity IDs, optionally filtered by substring";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1;
        error = valid ? null : "Usage: listentities [entity ID substring]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(autocomplete => AutocompleteHelpers.OptionalSingleFrom(autocomplete, Prefabs.Entities))
            .AutocompleteValidator(AutocompleteHelpers.ValidateOptionalSingleFilter);
    }

    public override void Execute(string[] args)
    {
        string result = Prefabs.Entities().Filter(args.FirstOrDefault() ?? "").Join();
        Accessors.CommandConsoleAccessor.EchoToConsole(
            string.IsNullOrWhiteSpace(result) ? "No matching entities" : $"- {result}");
    }

}
