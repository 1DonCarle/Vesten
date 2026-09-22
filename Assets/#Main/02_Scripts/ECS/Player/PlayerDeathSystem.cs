using Unity.Burst;
using Unity.Entities;

[UpdateAfter(typeof(DamageSystem))]
partial struct PlayerDeathSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var DestroyEntityLookup = SystemAPI.GetComponentLookup<DestroyEntityFlag>();
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (death, entity) in SystemAPI.Query<RefRW<Death>>().WithAll<PlayerTag>().WithEntityAccess())
        {
            if (!death.ValueRO.IsDead)
                continue;

            // TODO: Start Death Timer
            death.ValueRW.DestroyTime -= deltaTime;

            // TODO: Death animation / shader

            // TODO: Death VFX

            // TODO: SFX

            if (death.ValueRO.DestroyTime > 0)
            {
                continue;
            }
          
            // TODO: Open UI

            DestroyEntityLookup.SetComponentEnabled(entity, true);


        }
    }
}
