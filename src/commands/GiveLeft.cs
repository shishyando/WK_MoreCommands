using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GiveLeftCommand : CommandBase
{
    public override string[] Aliases => ["left"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "give item to left hand or inventory";
    public override bool CheatsOnly => true;

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            if (args.Length == 0)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Available items:\n{ItemGod.PrefabNames("\n")}");
                return;
            }
            var item = ItemGod.FindAndClone(args[0]);
            if (item == null)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"No such item: {args[0]}");
                return;
            }
            var clone = item.GetClone();
            clone.bagRotation = new UnityEngine.Quaternion(1, 2, 3, 4);
            Inventory.instance.AddItemToHand(clone, 0);
        };
    }
}
