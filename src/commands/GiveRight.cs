using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GiveRightCommand : CommandBase
{
    public override string[] Aliases => ["right"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "give item to right hand or inventory by its id with substring search";
    public override bool EnablesCheatsOnUse => true;

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
            Inventory inventory = GetInventoryOrWarn();
            if (inventory == null) return;

            Item clone = Prefabs.ItemProvider().FromCommand(args);
            if (clone == null) return;
            clone.bagRotation = new UnityEngine.Quaternion(1, 2, 3, 4);
            inventory.AddItemToHand(clone, 1);
        };
    }
}
