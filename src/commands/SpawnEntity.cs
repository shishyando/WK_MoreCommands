using System;
using MoreCommands.Common;
using UnityEngine;

namespace MoreCommands.Commands;


public sealed class SpawnEntityCommand : CommandBase
{
    public override string[] Aliases => ["spawn", "spawnentity"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Spawn entity by its prefab id with substring search";
    public override bool CheatsOnly => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            GameEntity e = Prefabs.EntityProvider().FromCommand(args);
            if (e == null) return;
            Vector3 position = Camera.main.transform.position + Camera.main.transform.forward;
            Quaternion rotation = Quaternion.identity;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit, float.PositiveInfinity))
            {
                position = raycastHit.point;
                rotation = Quaternion.LookRotation(raycastHit.normal);
                GameEntity gameEntity = UnityEngine.Object.Instantiate(e, position, rotation);
                if (gameEntity.spawnWithBounds)
                {
                    Collider component = gameEntity.GetComponent<Collider>();
                    if (component != null)
                    {
                        Bounds bounds = component.bounds;
                        Vector3 b = raycastHit.normal * bounds.extents.magnitude;
                        gameEntity.transform.position += b;
                    }
                }
            }
            else
            {
                UnityEngine.Object.Instantiate(e.gameObject, position, rotation);
            }
        };
    }

}
