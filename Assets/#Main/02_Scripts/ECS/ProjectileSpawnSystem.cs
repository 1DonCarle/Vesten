using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static PlayerAuthoring;

partial struct ProjectileSpawnSystem : ISystem
{
    public struct ProjectileSpawnRequest : IBufferElementData
    {
        public float3 Position;
        public float3 Direction;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

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

            }
            requests.Clear();
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

  
}
