using Unity.Burst;
using Unity.Entities;

partial struct ProcessDamageThisFrame : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (hitPoints, damageThisFrame) in SystemAPI.Query<RefRW<CharacterCurrentHitPoints>,DynamicBuffer<DamageThisFrame>>())
        {
            if (damageThisFrame.IsEmpty) continue;
            foreach (var damage in damageThisFrame)
            {
                hitPoints.ValueRW.Value -= damage.Value;
            }
            damageThisFrame.Clear();
        }
    }
}
