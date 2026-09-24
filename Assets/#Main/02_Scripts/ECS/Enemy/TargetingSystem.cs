using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using static PlayerAuthoring;

partial struct TargetingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out var playerEntity))
            return;
        var playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position.xz;


        foreach (var (target, transform) in SystemAPI.Query<RefRW<EnemyTarget>, LocalTransform>())
        {
            var enemyPosition = transform.Position.xz;
            target.ValueRW.Distance = Vector2.Distance(playerPosition, enemyPosition);
        }
    }


}
