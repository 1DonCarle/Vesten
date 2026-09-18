using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static EnemyAuthoring;
using static PlayerAuthoring;

[UpdateAfter(typeof(DecisionSystem))]
partial struct EnemyMoveToPlayerSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
  
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        var playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position.xz;
        var MoveToPlayerJob = new MoveEnemyJob
        {
            PlayerPosition = playerPosition
        };
        state.Dependency = MoveToPlayerJob.ScheduleParallel(state.Dependency);
    }

}
[BurstCompile]
[WithAll(typeof(EnemyTag))]
public partial struct MoveEnemyJob : IJobEntity
{
    public float2 PlayerPosition;
    public void Execute(ref CharacterMoveDirection direction, ref CharacterMoveSpeed speed, in LocalTransform transform, in EnemyBehaviourState state)
    {
        if (state.Value != EnemyBehaviour.Chase)
        {
            direction.Value = float2.zero;
            return;

        }           
        
        // Implement logic to move enemy towards the player
        var enemyPosition = transform.Position.xz;
        var toPlayer = PlayerPosition - enemyPosition;
        direction.Value = math.normalize(toPlayer);

  
    }
}
