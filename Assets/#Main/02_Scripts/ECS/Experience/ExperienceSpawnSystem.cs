using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

public struct ExperienceSpawnRequest : IBufferElementData
{    public float3 Position;
    public int Amount;
    }
partial struct ExperienceSpawnSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
    }

}
