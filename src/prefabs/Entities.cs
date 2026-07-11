using System;

namespace MoreCommands.HandleProviders;

public class Entities : HandleProvider<GameEntity>
{
    public override Func<GameEntity, string> Name { get; } = entity => entity?.entityPrefabID;

    public override Handle<GameEntity> Handle()
    {
        return new(CL_GameManager.gMan?.gameEntityPrefabs ?? [], Name);
    }
}
