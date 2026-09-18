using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using static PlayerAuthoring;


[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
[UpdateBefore(typeof(AfterPhysicsSystemGroup))]
partial struct EnemyCollideSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var elapsedTime = SystemAPI.Time.ElapsedTime;
        foreach (var (expirationTimestamp, cooldownEnabled) in SystemAPI.Query<EnemyCollisionIsOnCooldown, EnabledRefRW<EnemyCollisionIsOnCooldown>>())
        {
            if (expirationTimestamp.Value > elapsedTime) continue;
            // is off cooldown
            cooldownEnabled.ValueRW = false;
        }

        var collideJob = new EnemyCollideJob
        {
            PlayerLookup = SystemAPI.GetComponentLookup<PlayerTag>(true),
            EnemyCollideDataLookup = SystemAPI.GetComponentLookup<EnemyCollideData>(true),
            CooldownLookup = SystemAPI.GetComponentLookup<EnemyCollisionIsOnCooldown>(),
            DamageBufferLookup = SystemAPI.GetBufferLookup<DamageRequest>(),
            ElapsedTime = elapsedTime
        };

        // Schedule the job to run after the physics simulation has completed
        var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
        state.Dependency = collideJob.Schedule(simulationSingleton, state.Dependency);
    }
}

[BurstCompile]
public struct EnemyCollideJob : ICollisionEventsJob
{
    [ReadOnly] public ComponentLookup<PlayerTag> PlayerLookup;
    [ReadOnly] public ComponentLookup<EnemyCollideData> EnemyCollideDataLookup;
    public ComponentLookup<EnemyCollisionIsOnCooldown> CooldownLookup;
    public BufferLookup<DamageRequest> DamageBufferLookup;

    public double ElapsedTime;
    public void Execute(CollisionEvent collisionEvent)
    {
        Entity playerEntity;
        Entity enemyEntity;

        if (PlayerLookup.HasComponent(collisionEvent.EntityA) && EnemyCollideDataLookup.HasComponent(collisionEvent.EntityB))
        {
            playerEntity = collisionEvent.EntityA;
            enemyEntity = collisionEvent.EntityB;
        }
        else if (PlayerLookup.HasComponent(collisionEvent.EntityB) && EnemyCollideDataLookup.HasComponent(collisionEvent.EntityA))
        {
            playerEntity = collisionEvent.EntityB;
            enemyEntity = collisionEvent.EntityA;
        }
        else
        {
            return;
        }
        if (CooldownLookup.IsComponentEnabled(enemyEntity)) return;

        var collideData = EnemyCollideDataLookup[enemyEntity];
        CooldownLookup[enemyEntity] = new EnemyCollisionIsOnCooldown { Value = ElapsedTime + collideData.CollideCooldownTime };
        CooldownLookup.SetComponentEnabled(enemyEntity, true);

        var playerDamageBuffer = DamageBufferLookup[playerEntity];

        
        playerDamageBuffer.Add(new DamageRequest {
            Target = playerEntity,
            Source = enemyEntity,
            Damage = collideData.CollideDamage
             });
    }
}