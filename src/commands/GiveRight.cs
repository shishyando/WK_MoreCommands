using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GiveRightCommand : CommandBase
{
    public override string[] Aliases => ["right"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Give an item to the right hand by exact ID; no argument lists items";
    public override bool EnablesCheatsOnUse => true;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1
            && (args.Length == 0 || AutocompleteHelpers.HasExactMatch(Prefabs.Items, args[0]));
        error = valid ? null : "Usage: right [exact item ID]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(autocomplete => AutocompleteHelpers.OptionalSingleFrom(autocomplete, Prefabs.Items))
            .AutocompleteValidator(validator => AutocompleteHelpers.ValidateOptionalSingleExactFrom(validator, Prefabs.Items));
    }

    public override void Execute(string[] args)
    {
        Inventory inventory = GetInventoryOrWarn();
        if (inventory == null) return;

        Item clone = Prefabs.ItemProvider().FromCommand(args);
        if (clone == null) return;
        inventory.AddItemToHand(clone, 1);
    }
}
