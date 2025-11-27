using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class BanhammerCommand : CommandBase
{
    public override string[] Aliases => ["banhammer"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "give banhammer to player";
    public override bool CheatsOnly => true;

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Inventory.instance.AddItemToHand(PrefabsItems.GetItemByPrefabName("item_banhammer").GetClone(), 0);
        };
    }
}
