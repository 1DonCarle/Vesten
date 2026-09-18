using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static PlayerAuthoring;
using static ProjectileSpawnSystem;

// I think this system will shoot towards mouse position when attack is called.
partial struct PlayerAttackSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        var buffer = SystemAPI.GetBuffer<ProjectileSpawnRequest>(playerEntity);

        foreach (var commands in SystemAPI.Query<RefRO<PlayerCommands>>())
        {
            if (!commands.ValueRO.Attack)
                continue;

            float3 spawnPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position; // muzzle position

            float3 direction = math.normalize(commands.ValueRO.PointerWorldPosition - spawnPosition);
            buffer.Add(new ProjectileSpawnRequest
            {
                Position = spawnPosition,
                Direction = direction
            });
            // Shoot towards this



            // Add projectile to buffer system

            // Create VFX

            // Subtract bullets



        }
    }


}
