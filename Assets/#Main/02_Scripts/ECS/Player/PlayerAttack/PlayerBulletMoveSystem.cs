using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct PlayerBulletMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, data) in
          SystemAPI.Query<RefRW<LocalTransform>, BulletData>())
        {
            transform.ValueRW.Position += transform.ValueRO.Forward() * data.Speed * deltaTime;
        }
    }

 
}
