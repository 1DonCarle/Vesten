using Unity.Burst;
using Unity.Entities;

[UpdateAfter(typeof(PlayerAttackSystem))]
partial struct PlayerReloadSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var attackData in SystemAPI.Query<RefRW<PlayerAttackData>>())
        {
            if (attackData.ValueRW.CurrentBullets > 0)
                continue;

            attackData.ValueRW.ReloadCooldown -= deltaTime;

            if (attackData.ValueRW.ReloadCooldown > 0f)
                continue;


            // Reload magasine and reset timer
            attackData.ValueRW.ReloadCooldown = attackData.ValueRW.ReloadTime;
            attackData.ValueRW.CurrentBullets = attackData.ValueRW.MagasineSize;

        }
    }


}
