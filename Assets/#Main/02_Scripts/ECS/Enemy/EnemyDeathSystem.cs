using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

[UpdateAfter(typeof(DamageSystem))]
partial struct EnemyDeathSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var DestroyEntityLookup = SystemAPI.GetComponentLookup<DestroyEntityFlag>();
        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (death, enemyBehaviourState,experience, entity) in SystemAPI.Query<RefRW<Death>, RefRW<EnemyBehaviourState>, RefRW<ExperienceAmount>>().WithAll<EnemyTag>().WithEntityAccess())
        {

            if (!death.ValueRO.IsDead)
                continue;
            if (!death.ValueRO.DeathStarted)
            {
                // Use a one time flag to ensure that the death sequence is only triggered once
                death.ValueRW.DeathStarted = true;

                // TODO: Disable movement / attack / etc
                enemyBehaviourState.ValueRW.Value = EnemyBehaviour.Dying;

                // TODO: Disable physics / collisions
                SystemAPI.SetComponent(entity, new PhysicsCollider { Value = default });


                // Start animation
                // VFX
                // SFX
            }

            // TODO: Start Death Timer
            death.ValueRW.DestroyTime -= deltaTime;

            if (death.ValueRO.DestroyTime > 0)
            {
                continue;
            }
            // TODO: Drop loot / XP / etc
            var buffer = SystemAPI.GetBuffer<ExperienceSpawnRequest>(entity);
            buffer.Add(new ExperienceSpawnRequest
            {
                Position = SystemAPI.GetComponent<LocalTransform>(entity).Position,
                Amount = experience.ValueRO.Value
            });

            DestroyEntityLookup.SetComponentEnabled(entity, true);


        }
    }
}