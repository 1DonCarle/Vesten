using Unity.Entities;
using UnityEngine;
public struct HitEnemy : IBufferElementData
{
    public Entity Enemy;
}
public struct BulletLifeTimestamp : IComponentData
{
    public double Value;
}

public class PlayerBulletAuthoring : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float bulletDamage = 1f;
    public float bulletLifeTime = 2f;
    public int bulletPenetration = 1;

    public class PlayerBulletAuthoringBaker : Baker<PlayerBulletAuthoring>
    {
      

        public override void Bake(PlayerBulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new BulletData
            {
                Speed = authoring.bulletSpeed,
                Damage = authoring.bulletDamage,
                LifeTime = authoring.bulletLifeTime,
                BulletPenetration = authoring.bulletPenetration
            });
            AddComponent<BulletLifeTimestamp>(entity);
            AddBuffer<HitEnemy>(entity);
            AddComponent<DestroyEntityFlag>(entity);
            SetComponentEnabled<DestroyEntityFlag>(entity, false);
        }
    }
}

