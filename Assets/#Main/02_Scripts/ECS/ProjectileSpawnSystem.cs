using NUnit;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
public struct ProjectileSpawnRequest : IBufferElementData
{
    public float3 Position;
    public float3 Direction;
}
public struct VFXDestroyBulletEvent : IBufferElementData
{
    public uint ProjectileId;
}
public struct ProjectileIdCounter : IComponentData
{
    public uint NextId;
}
public partial struct ProjectileIdCounterSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var entity = state.EntityManager.CreateEntity();

        state.EntityManager.AddComponentData(entity, new ProjectileIdCounter
        {
            NextId = 1
        });
    }
}
partial struct ProjectileSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var elapsedTime = SystemAPI.Time.ElapsedTime;

        var singletonEntity = SystemAPI.GetSingletonEntity<VFXPlayerAttackSingleton>();
        var vfxSingleton = state.EntityManager.GetComponentData<VFXPlayerAttackSingleton>(singletonEntity);

        // Get our unique projectile ID counter
        var counterEntity = SystemAPI.GetSingletonEntity<ProjectileIdCounter>();
        var counter = state.EntityManager.GetComponentData<ProjectileIdCounter>(counterEntity);

        foreach (var (player, requests) in
                 SystemAPI.Query<RefRO<PlayerData>, DynamicBuffer<ProjectileSpawnRequest>>())
        {
            foreach (var request in requests)
            {
                // Generate a unique ID for this projectile
                uint projectileId = counter.NextId++;

                Entity bullet = ecb.Instantiate(player.ValueRO.BulletPrefab);

                ecb.SetComponent(
                    bullet,
                    LocalTransform.FromPositionRotation(
                        request.Position,
                        quaternion.LookRotationSafe(
                            request.Direction,
                            math.up()
                        )
                    )
                );

                var bulletData =
                    state.EntityManager.GetComponentData<BulletData>(
                        player.ValueRO.BulletPrefab
                    );

                bulletData.ProjectileId = projectileId;

                ecb.SetComponent(bullet, bulletData);

                ecb.SetComponent(
                    bullet,
                    new BulletLifeTimestamp
                    {
                        Value = elapsedTime + bulletData.LifeTime
                    }
                );

                // Give the VFX projectile the SAME ID
                var req = new VFXPlayerAttackRequest
                {
                    Position = request.Position,
                    Direction = request.Direction * bulletData.Speed,
                    Color = new float3(1f, 0.5f, 0f),
                    Lifetime = bulletData.LifeTime,
                    Damage = bulletData.Damage,
                    ProjectileId = projectileId
                };

                vfxSingleton.Manager.AddRequest(req);
            }

            requests.Clear();
        }

        // Save the incremented counter
        state.EntityManager.SetComponentData(counterEntity, counter);

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}