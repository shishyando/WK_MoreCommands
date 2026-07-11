using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class BanhammerCommand : CommandBase
{
    public override string[] Aliases => ["banhammer"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Give the player a banhammer in the left hand";
    public override bool EnablesCheatsOnUse => true;

    public override void Execute(string[] args)
    {
        Inventory inventory = GetInventoryOrWarn();
        if (inventory == null) return;

        Item banhammer = Prefabs.Items().FilterExact("item_banhammer").Any()?.GetClone();
        if (banhammer == null) return;

        inventory.AddItemToHand(banhammer, 0);
        Accessors.CommandConsoleAccessor.EchoToConsole($"{Colors.Tagged("banhammer", Colors.REDDISH)} given");
    }
}
