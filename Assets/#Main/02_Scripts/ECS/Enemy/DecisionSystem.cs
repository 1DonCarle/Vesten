using Unity.Burst;
using Unity.Entities;

[UpdateAfter(typeof(TargetingSystem))]
partial struct DecisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (behaviourState, target, attackData, config) in SystemAPI.Query<RefRW<EnemyBehaviourState>, RefRO<EnemyTarget>, RefRO<EnemyAttackData>, RefRO<EnemyConfig>>())
        {
            switch(behaviourState.ValueRO.Value)
            {
                case EnemyBehaviour.Idle:
                    // Default state.
                    if (target.ValueRO.Distance <= config.ValueRO.DetectionRange)
                    { 
                        behaviourState.ValueRW.Value = EnemyBehaviour.Chase; 
                    }
                    break;
                case EnemyBehaviour.Chase:
                    if (target.ValueRO.Distance<attackData.ValueRO.AttackRange)
                    {
                        behaviourState.ValueRW.Value = EnemyBehaviour.Aim;
                    }
                    //UpdateChaseState(behaviourState, target, attackData);
                    break;
                case EnemyBehaviour.Aim:
                    //UpdateAimState(behaviourState, target, attackData);
                    break;
                case EnemyBehaviour.Attack:
                    //UpdateAttackState(behaviourState, target, attackData);
                    break;
                case EnemyBehaviour.Reload:
                   // UpdateReloadState(behaviourState, target, attackData);
                    break;
                case EnemyBehaviour.Flee:
                    //UpdateFleeState(behaviourState, target, attackData);
                    break;
            }




     
        }
    } 
}
