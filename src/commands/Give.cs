using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GiveCommand : CommandBase
{
    public override string[] Aliases => ["give"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "give item to player's inventory by its id with substring search";
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
            inventory.AddItemToInventoryScreen(new UnityEngine.Vector3(0f, 0f, 1f) + UnityEngine.Random.insideUnitSphere * 0.01f, clone, true, false);
        };
    }
}
