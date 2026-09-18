using Unity.Entities;
using UnityEngine;

class PlayerBulletAuthoring : MonoBehaviour
{
    
}public struct HitEnemy : IBufferElementData
{
    public Entity Enemy;
}
public struct BulletLifeTimestamp : IComponentData
{
    public double Value;
}

class PlayerBulletAuthoringBaker : Baker<PlayerBulletAuthoring>
{
    public override void Bake(PlayerBulletAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new BulletData
      {
          Speed = 10f,
          Damage = 1f,
          LifeTime = 5f,
          BulletPenetration = 1
        });
        AddComponent<BulletLifeTimestamp>(entity);
        AddBuffer<HitEnemy>(entity);
    }
}
