using System;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class AlwaysSpawnCommand : TogglableCommandBase
{
    public override string[] Aliases => ["alwaysspawn"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Guarantee spawns (items, handholds, supply crates, etc.)";
    public override bool CheatsOnly => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            Patches.UT_SpawnChance_Start_Patcher.AlwaysSpawn = Enabled;
        };
    }
}
