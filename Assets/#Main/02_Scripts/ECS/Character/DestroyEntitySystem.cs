using System.Diagnostics;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using static PlayerAuthoring;

public struct DestroyEntityFlag : IComponentData, IEnableableComponent { }
[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
[UpdateBefore(typeof(EndSimulationEntityCommandBufferSystem))]
public partial struct DestroyEntitySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }
    public void OnUpdate(ref SystemState state)
    {
        var ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var endEcb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);

        var ecbBeginSystem = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var beginEcb = ecbBeginSystem.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (_, entity) in SystemAPI.Query<DestroyEntityFlag>().WithEntityAccess())
        {
            if (SystemAPI.HasComponent<PlayerTag>(entity))
            {
                UnityEngine.Debug.Log("GAME OVER");

            }
            else if (SystemAPI.HasComponent<EnemyTag>(entity))
            {
                //var coinPrefab = SystemAPI.GetComponent<CoinPrefab>(entity).Value;
                //var newGem = beginEcb.Instantiate(coinPrefab);
                var spawnPosition = SystemAPI.GetComponent<LocalTransform>(entity).Position;
               // beginEcb.SetComponent(newGem, LocalTransform.FromPosition(spawnPosition));

            }
            endEcb.DestroyEntity(entity);
        }

    }
}