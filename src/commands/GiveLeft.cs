using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GiveLeftCommand : CommandBase
{
    public override string[] Aliases => ["left"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "give item to left hand or inventory by its id with substring search";
    public override bool CheatsOnly => true;

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Item clone = PrefabsItems.ItemCloneForCommandFromArgs(args);
            if (clone == null) return;
            clone.bagRotation = new UnityEngine.Quaternion(1, 2, 3, 4);
            Inventory.instance.AddItemToHand(clone, 0);
        };
    }
}
