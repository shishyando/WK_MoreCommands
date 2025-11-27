using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class GiveCommand : CommandBase
{
    public override string[] Aliases => ["give"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "give item to player";
    public override bool CheatsOnly => true;

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            if (args.Length == 0)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Available items:\n{PrefabsItems.ItemPrefabNames("\n")}");
                return;
            }
            var clone = PrefabsItems.FindAndCloneItem(args[0]);
            if (clone == null)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"No such item: {args[0]}");
                return;
            }
            Inventory.instance.AddItemToInventoryScreen(new UnityEngine.Vector3(0f, 0f, 1f) + UnityEngine.Random.insideUnitSphere * 0.01f, clone, true, false);
        };
    }
}
