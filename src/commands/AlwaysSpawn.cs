using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class AlwaysSpawnCommand : TogglableCommandBase
{
    public override string[] Aliases => ["alwaysspawn"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Force eligible future spawn-chance checks to succeed and preserve their success/failure events";
    public override bool EnablesCheatsOnUse => true;

    public override void Execute(string[] args)
    {
        Patches.UT_SpawnChance_Start_Patcher.AlwaysSpawn = Enabled;
    }
}
