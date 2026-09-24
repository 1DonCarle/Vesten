using Unity.Entities;
using UnityEngine;


public struct EnemyTag : IComponentData { }
public struct EnemyCollideData : IComponentData
{
    public int CollideDamage;
    public float CollideCooldownTime;
}
public struct EnemyCollisionIsOnCooldown : IComponentData, IEnableableComponent
{
    public double Value;
}

public struct EnemyAttackData : IComponentData
{
    public int AttackDamage;
    public float AttackRange;
    public float ReloadTime;
    public float TimeToAim;

    public float ReloadCooldown;
    public float AimCooldownTimer;
    public float MagasineSize;
    public float CurrentBullets;
}
public struct EnemyConfig : IComponentData
{
    public AttackType AttackType;
    public float DetectionRange;
}
public struct ExperienceAmount : IComponentData
{
    public int Value;
}
public struct EnemyTarget : IComponentData
{
    public Entity Target;
    public float Distance;
}
[RequireComponent(typeof(CharacterAuthoring))]
public class EnemyAuthoring : MonoBehaviour
{
    public int CollideDamage;
    public float CollideCooldownTime;
    public AttackType AttackType;

    public float ReloadTime;
    public float AttackRange;
    public int AttackDamage;
    public float TimeToAim;

    public float DetectionRange = 10f;

    public float MagasineSize=6f;

public int ExperienceAmount=10;
    private class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<EnemyTag>(entity);
            AddComponent(entity, new EnemyCollideData
            {
                CollideDamage = authoring.CollideDamage,
                CollideCooldownTime = authoring.CollideCooldownTime
            });
            AddComponent<EnemyCollisionIsOnCooldown>(entity);
            SetComponentEnabled<EnemyCollisionIsOnCooldown>(entity, false);

            AddComponent(entity, new EnemyBehaviourState
            {
                Value = EnemyBehaviour.Idle
            });
            AddComponent(entity, new EnemyTarget
            {
                Target = Entity.Null,
                Distance = float.MaxValue
            });

            switch (authoring.AttackType)
            {
                case AttackType.Melee:
                    AddComponent(entity, new EnemyAttackData
                    {
                        AttackDamage = authoring.AttackDamage,
                        AttackRange = authoring.AttackRange,
                        ReloadTime = authoring.ReloadTime,
                        AimCooldownTimer = authoring.TimeToAim,
                        TimeToAim = authoring.TimeToAim,
                        ReloadCooldown = authoring.ReloadTime,
                        MagasineSize = authoring.MagasineSize,
                        CurrentBullets = authoring.MagasineSize
                    });
                    break;
                case AttackType.Ranged:
                    AddComponent(entity, new EnemyAttackData
                    {
                        AttackDamage = authoring.AttackDamage,
                        AttackRange = authoring.AttackRange,
                        ReloadTime = authoring.ReloadTime,
                        AimCooldownTimer = authoring.TimeToAim, 
                        TimeToAim = authoring.TimeToAim,
                        ReloadCooldown = authoring.ReloadTime,
                        MagasineSize = authoring.MagasineSize,
                        CurrentBullets = authoring.MagasineSize
                    });
                    break;
                case AttackType.None:
                    break;
            }
            AddComponent(entity, new EnemyConfig
            {
                AttackType = authoring.AttackType,
                DetectionRange = authoring.DetectionRange
            });
            AddComponent(entity, new ExperienceAmount
            {
                Value = authoring.ExperienceAmount
            });
        }
    }
}
