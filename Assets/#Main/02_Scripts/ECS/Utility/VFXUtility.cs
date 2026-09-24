using UnityEngine;
using UnityEngine.VFX;

[VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
public struct VFXEnemyAttackRequest
{
    // The request also the one used in VFX graph
    public Vector3 Position;
    public Vector3 Color;
    public float Damage;
}
[VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
public struct VFXImpactRequest
{
    // The request also the one used in VFX graph
    public Vector3 Position;
    public Vector3 Color;
    public float Damage;
}
[VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
public struct VFXOnDeathRequest
{
    // The request also the one used in VFX graph
    public Vector3 Position;
    public Vector3 Color;
    public float Damage;
}
[VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
public struct VFXPlayerAttackRequest
{
    // The request also the one used in VFX graph
    public Vector3 Position;
    public Vector3 Direction;
    public Vector3 Color;
    public float Lifetime;
    public float Damage;
    public uint ProjectileId;
}
[VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
public struct VFXDestroyBulletRequest
{
    public uint ProjectileId;
}
public static class VFXReferences
{
    public static VisualEffect MuzzleFlashGraph;
    public static GraphicsBuffer MuzzleFlashRequestBuffer;

    public static VisualEffect ImpactGraph;
    public static GraphicsBuffer ImpactRequestBuffer;

    public static VisualEffect BloodSplatterGraph;
    public static GraphicsBuffer BloodSplatterRequestBuffer;

    public static VisualEffect PlayerAttackGraph;
    public static GraphicsBuffer PlayerAttackRequestBuffer;
    public static GraphicsBuffer DestroyBulletRequestBuffer;

}
