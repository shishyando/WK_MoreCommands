using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class BanhammerCommand : CommandBase
{
    public override string[] Aliases => ["banhammer"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "give banhammer to player";
    public override bool CheatsOnly => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Inventory.instance.AddItemToHand(Prefabs.Items().Filter("item_banhammer").Any(), 0);
            Accessors.CommandConsoleAccessor.EchoToConsole($"{Colors.Tagged("banhammer", Colors.REDDISH)} given");
        };
    }
}
