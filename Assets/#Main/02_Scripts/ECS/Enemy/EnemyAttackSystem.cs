using System;
using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static PlayerAuthoring;

partial struct EnemyMeleeAttackSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        Entity player = SystemAPI.GetSingletonEntity<PlayerTag>();
        RefRW<CharacterCurrentHitPoints> playerHP = SystemAPI.GetComponentRW<CharacterCurrentHitPoints>(player);
        float3 playerPosition = SystemAPI.GetComponent<LocalTransform>(player).Position;

        var singletonEntity = SystemAPI.GetSingletonEntity<VFXEnemyAttackSingleton>();
        var vfxSingleton = state.EntityManager.GetComponentData<VFXEnemyAttackSingleton>(singletonEntity);

        foreach (var (attackData, enemyTransform, enemyState, entity) in SystemAPI.Query<RefRW<EnemyAttackData>, RefRO<LocalTransform>, RefRW<EnemyBehaviourState>>().WithEntityAccess())
        {
            if (enemyState.ValueRO.Value == EnemyBehaviour.Aim)
            {
                attackData.ValueRW.AimCooldownTimer -= deltaTime;
                if (attackData.ValueRW.AimCooldownTimer > 0) continue;
                attackData.ValueRW.AimCooldownTimer = attackData.ValueRO.TimeToAim;
                enemyState.ValueRW.Value = EnemyBehaviour.Attack;



            }
            else if (enemyState.ValueRO.Value == EnemyBehaviour.Attack)
            {

                float distanceToPlayer = math.distance(playerPosition.xz, enemyTransform.ValueRO.Position.xz);
                // Do all the attack logic here. Then subtract a bullet and if the bullets are 0, set the state to reload. If the player is hit, subtract health from the player.
                // Create a request 
                var req = new VFXEnemyAttackRequest
                {
                    Position = SystemAPI.GetComponent<LocalTransform>(entity).Position,
                    Color = new float3(1f, 0.5f, 0f),
                    Damage = 10f
                };

                // Add the request
                vfxSingleton.Manager.AddRequest(req);

                attackData.ValueRW.CurrentBullets--;
                if (attackData.ValueRW.CurrentBullets <= 0)
                    enemyState.ValueRW.Value = EnemyBehaviour.Reload;
                else
                    enemyState.ValueRW.Value = EnemyBehaviour.Chase;




            }
            else if (enemyState.ValueRO.Value == EnemyBehaviour.Reload)
            {
                attackData.ValueRW.ReloadCooldown -= deltaTime;
                if (attackData.ValueRW.ReloadCooldown > 0) continue;
                attackData.ValueRW.ReloadCooldown = attackData.ValueRO.ReloadTime;
                attackData.ValueRW.CurrentBullets = attackData.ValueRO.MagasineSize;


                enemyState.ValueRW.Value = EnemyBehaviour.Chase;


            }



            // Set state to aim

            //Only take damage if the player is hit
            //playerHP.ValueRW.Value -= attackData.ValueRO.AttackDamage;



        }



    }
}
