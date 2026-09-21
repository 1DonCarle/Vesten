using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using static ProjectileSpawnSystem;

public struct InitializeCharacterFlag : IComponentData, IEnableableComponent{}
#region // Character Components
public struct CharacterMoveDirection : IComponentData
{
    public float2 Value;
}
public struct CharacterMoveSpeed : IComponentData
{
    public float Value;
}
public struct FaceDirection : IComponentData
{
    public float2 Value;
}
public struct CharacterMaxHitPoints : IComponentData
{
    public int Value;
}
public struct CharacterCurrentHitPoints : IComponentData
{
    public int Value;
}
public struct IsDead : IComponentData
{
    public bool Value;
}
#endregion
public class CharacterAuthoring : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public int HitPoints = 100;
    private class Baker : Baker<CharacterAuthoring>
    {
        public override void Bake(CharacterAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<InitializeCharacterFlag>(entity);
            AddComponent<CharacterMoveDirection>(entity);
            AddComponent(entity, new CharacterMoveSpeed
            {
                Value = authoring.MoveSpeed
            });
            AddComponent<FaceDirection>(entity);
            AddComponent(entity, new CharacterMaxHitPoints
            {
                Value = authoring.HitPoints
            });
            AddComponent(entity, new CharacterCurrentHitPoints
            {
                Value = authoring.HitPoints
            });
            AddBuffer<DamageRequest>(entity);
            AddBuffer<ProjectileSpawnRequest>(entity);
            AddComponent(entity, new IsDead
            {
                Value = false
            });
            AddComponent<DestroyEntityFlag>(entity);
            SetComponentEnabled<DestroyEntityFlag>(entity, false);
        }
    }
}