using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ExploreCommand : TogglableCommandBase
{
    public override string[] Aliases => ["explore"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Toggle freerun and noclip together";
    public override bool EnablesCheatsOnUse => true;

    public override void Execute(string[] args)
    {
        bool flashEnabled = CommandRegistry.GetCommand<FlashCommand>() is ITogglableCommand flash && flash.Enabled;
        CommandRegistry.SetTogglable<FreerunCommand>(Enabled || flashEnabled);
        ENT_Player player = ENT_Player.playerObject;
        player?.Noclip(WhenEnabled(Enabled));
    }
}
