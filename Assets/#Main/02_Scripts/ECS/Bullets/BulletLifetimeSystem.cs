using Unity.Burst;
using Unity.Entities;

partial struct BulletLifetimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency.Complete();

        var elapsedTime = SystemAPI.Time.ElapsedTime;
        var destroyEntityLookup = SystemAPI.GetComponentLookup<DestroyEntityFlag>();

        foreach (var (lifeTimestamp, entity) in SystemAPI.Query<RefRO<BulletLifeTimestamp>>().WithEntityAccess())
        {
            if (elapsedTime >= lifeTimestamp.ValueRO.Value)
            {
                destroyEntityLookup.SetComponentEnabled(entity, true);
            }
        }

    }
}
