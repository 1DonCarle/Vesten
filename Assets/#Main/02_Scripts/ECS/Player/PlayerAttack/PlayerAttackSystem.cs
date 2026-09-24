using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


// I think this system will shoot towards mouse position when attack is called.
partial struct PlayerAttackSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
            if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out var playerEntity))
            return;
        var buffer = SystemAPI.GetBuffer<ProjectileSpawnRequest>(playerEntity);




        foreach (var (commands, attackData) in SystemAPI.Query<RefRO<PlayerCommands>, RefRW<PlayerAttackData>>())
        {
            if (!commands.ValueRO.Attack)
                continue;

            // Check if attack is on cooldown
            if (attackData.ValueRW.CurrentBullets <= 0f)
            {
                continue;
            }
            // From here
            float3 spawnPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

            // Shoot towards this
            float3 direction = math.normalize(new float3(commands.ValueRO.PointerWorldPosition.x, 0, commands.ValueRO.PointerWorldPosition.z) - new float3(spawnPosition.x, 0, spawnPosition.z));

            // Create projectile entity
            buffer.Add(new ProjectileSpawnRequest
            {
                Position = spawnPosition,
                Direction = direction
            });




            // Subtract bullets
            attackData.ValueRW.CurrentBullets--;
        }
    }


}
