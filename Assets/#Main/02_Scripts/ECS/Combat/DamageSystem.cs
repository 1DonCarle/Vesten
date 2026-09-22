using Unity.Burst;
using Unity.Entities;
[UpdateAfter(typeof(BulletHitJob))]
partial struct DamageSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
     foreach(var (death, hitPoints, requests) in SystemAPI.Query<RefRW<Death>, RefRW<CharacterCurrentHitPoints>, DynamicBuffer<DamageRequest>>())
        {
            foreach (var request in requests)
            {
                hitPoints.ValueRW.Value -= (int)request.Damage;
            }
            requests.Clear();
            if (hitPoints.ValueRW.Value <= 0)
            {
                death.ValueRW.IsDead= true;
            }
        }
    }

}
