using Unity.Entities;


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

            endEcb.DestroyEntity(entity);
        }

    }
}