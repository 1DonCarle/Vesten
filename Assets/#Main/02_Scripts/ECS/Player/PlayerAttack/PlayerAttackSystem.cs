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
        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (commands, attackData) in SystemAPI.Query<RefRO<PlayerCommands>, RefRW<PlayerAttackData>>())
        {
            if (!commands.ValueRO.Attack)
                continue;

            // Check if attack is on cooldown
            if (attackData.ValueRW.CurrentBullets <= 0f)
            {
                continue;
            }
            float3 spawnPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position; // muzzle position

            float3 direction = math.normalize(new float3(commands.ValueRO.PointerWorldPosition.x, 0, commands.ValueRO.PointerWorldPosition.z) - new float3(spawnPosition.x, 0, spawnPosition.z));
            buffer.Add(new ProjectileSpawnRequest
            {
                Position = spawnPosition,
                Direction = direction
            });
            // Shoot towards this



            // Add projectile to buffer system

            // Create VFX

            // Subtract bullets
            attackData.ValueRW.CurrentBullets--;
        }
    }


}
