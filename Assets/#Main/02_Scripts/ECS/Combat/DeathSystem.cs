using Unity.Burst;
using Unity.Entities;

[UpdateAfter(typeof(DamageSystem))]
partial struct DeathSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var DestroyEntityLookup = SystemAPI.GetComponentLookup<DestroyEntityFlag>();

        foreach (var (death, entity) in SystemAPI.Query<RefRW<IsDead>>().WithEntityAccess())
        {
            if (death.ValueRO.Value)
            {
                // TODO: Death animation / shader

                // TODO: Death VFX

                // TODO: Drop loot

                // TODO: SFX

                DestroyEntityLookup.SetComponentEnabled(entity, true);

            }
        }
    }
}
