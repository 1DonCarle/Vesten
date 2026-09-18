using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

public partial struct BulletHitJob : ITriggerEventsJob
{
    public ComponentLookup<BulletData> BulletLookup;
    [ReadOnly] public ComponentLookup<EnemyTag> EnemyLookup;
    public BufferLookup<DamageRequest> DamageRequestLookup;
    public ComponentLookup<DestroyEntityFlag> DestroyEntityLookup;
    public BufferLookup<HitEnemy> HitEnemyBufferLookup;
    public float DeltaTime;
    public void Execute(TriggerEvent triggerEvent)
    {
        // Implement bullet collision logic here
        Entity bulletEntity;
        Entity enemyEntity;

        if (BulletLookup.HasComponent(triggerEvent.EntityA) && EnemyLookup.HasComponent(triggerEvent.EntityB))
        {
            bulletEntity = triggerEvent.EntityA;
            enemyEntity = triggerEvent.EntityB;
        }
        else if (BulletLookup.HasComponent(triggerEvent.EntityB) && EnemyLookup.HasComponent(triggerEvent.EntityA))
        {
            bulletEntity = triggerEvent.EntityB;
            enemyEntity = triggerEvent.EntityA;
        }
        else
        {
            return; // No bullet-enemy collision
        }
        var hitBuffer = HitEnemyBufferLookup[bulletEntity];
        // Check if this enemy was already hit
        for (int i = 0; i < hitBuffer.Length; i++)
        {
            if (hitBuffer[i].Enemy == enemyEntity)
                return; // Already hit, skip
        }
        // Not hit yet, apply damage and record
        hitBuffer.Add(new HitEnemy { Enemy = enemyEntity });

        var attackDamage = BulletLookup[bulletEntity].Damage;
        var bulletData = BulletLookup[bulletEntity];


        bulletData.BulletPenetration--;
        BulletLookup[bulletEntity] = bulletData;

        if (bulletData.BulletPenetration <= 0)
        {
            DestroyEntityLookup.SetComponentEnabled(bulletEntity, true);
        }

    }
}
