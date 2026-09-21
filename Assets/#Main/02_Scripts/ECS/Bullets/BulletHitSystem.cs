using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
[UpdateBefore(typeof(AfterPhysicsSystemGroup))]
partial struct BulletHitSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var elapsedTime = SystemAPI.Time.ElapsedTime;

        var attackJob = new BulletHitJob
        {
            BulletLookup = SystemAPI.GetComponentLookup<BulletData>(),
            EnemyLookup = SystemAPI.GetComponentLookup<EnemyTag>(true),
            DamageRequestLookup = SystemAPI.GetBufferLookup<DamageRequest>(),
            DestroyEntityLookup = SystemAPI.GetComponentLookup<DestroyEntityFlag>(),
            HitEnemyBufferLookup = SystemAPI.GetBufferLookup<HitEnemy>(),
        };
        var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
        state.Dependency = attackJob.Schedule(simulationSingleton, state.Dependency);
    }
}



