using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
partial struct CharacterMoveSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var(velocity,transform, direction,speed) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<LocalTransform>, CharacterMoveDirection, CharacterMoveSpeed>())
        {
            var moveStep2d = direction.Value * speed.Value;
            velocity.ValueRW.Linear = new float3(moveStep2d.x, 0f,moveStep2d.y);
            //Handles rotation of the character based on the direction of movement
            if (moveStep2d.x != 0f || moveStep2d.y != 0f)
            transform.ValueRW.Rotation = quaternion.Euler(0f, math.atan2(direction.Value.x, direction.Value.y), 0f);
        }
    }
}
