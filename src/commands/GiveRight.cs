using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GiveRightCommand : CommandBase
{
    public override string[] Aliases => ["right"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "give item to right hand or inventory by its id with substring search";
    public override bool CheatsOnly => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Item clone = PrefabsItems.ItemCloneForCommandFromArgs(args);
            if (clone == null) return;
            clone.bagRotation = new UnityEngine.Quaternion(1, 2, 3, 4);
            Inventory.instance.AddItemToHand(clone, 1);
        };
    }
}
