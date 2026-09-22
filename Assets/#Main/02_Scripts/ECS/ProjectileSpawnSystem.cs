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

partial struct ProjectileSpawnSystem : ISystem
{
 
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var elapsedTime = SystemAPI.Time.ElapsedTime;

        foreach (var (player, requests) in SystemAPI.Query<RefRO<PlayerData>, DynamicBuffer<ProjectileSpawnRequest>>())
        {
            foreach (var request in requests)
            {
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
                var bulletData = state.EntityManager.GetComponentData<BulletData>(player.ValueRO.BulletPrefab);

                ecb.SetComponent(bullet, new BulletLifeTimestamp { Value = elapsedTime + bulletData.LifeTime });


            }
            requests.Clear();
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }


}
