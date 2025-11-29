using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GiveCommand : CommandBase
{
    public override string[] Aliases => ["give"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "give item to player's inventory by its id with substring search";
    public override bool CheatsOnly => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Item clone = PrefabsItems.ItemCloneForCommandFromArgs(args);
            if (clone == null) return;
            Inventory.instance.AddItemToInventoryScreen(new UnityEngine.Vector3(0f, 0f, 1f) + UnityEngine.Random.insideUnitSphere * 0.01f, clone, true, false);
        };
    }
}
