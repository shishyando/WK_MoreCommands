using MoreCommands.Common;

namespace MoreCommands.Commands;

public sealed class FreezeAiCommand : TogglableCommandBase
{
    public override string[] Aliases => ["freezeai"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Pause AI update loops and targeting, including tentacles; physics, animations, and running coroutines continue";
    public override bool EnablesCheatsOnUse => true;

    public override void Execute(string[] args)
    {
        FreezeAiController.SetFrozen(Enabled);
    }
}
