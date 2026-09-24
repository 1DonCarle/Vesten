using Unity.Entities;

public struct BulletData : IComponentData
{
    public float Speed;
    public float LifeTime;
    public float Damage;
    public int BulletPenetration;
    public uint ProjectileId;
}