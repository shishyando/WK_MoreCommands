using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class FlashCommand : TogglableCommandBase
{
    public override string[] Aliases => ["flash"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Toggle freerun and the all-purpose buff together";
    public override bool EnablesCheatsOnUse => true;

    public override void Execute(string[] args)
    {
        bool exploreEnabled = CommandRegistry.GetCommand<ExploreCommand>() is ITogglableCommand explore && explore.Enabled;
        CommandRegistry.SetTogglable<FreerunCommand>(Enabled || exploreEnabled);
        CommandRegistry.SetTogglable<BuffCommand>(Enabled);
    }
}
