using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
public struct PlayerTag : IComponentData
{

}
public struct PlayerCommands : IComponentData
{
    public float2 Move;
    public bool Attack;

    public float3 PointerWorldPosition;
}
public struct ECSCameraTarget : IComponentData
{
    public UnityObjectRef<Transform> CameraTransform;
}
public struct PlayerData : IComponentData
{
    public Entity BulletPrefab;

}
public struct PlayerAttackData: IComponentData
{
    public int CurrentBullets;
    public int MagasineSize;
    public float ReloadCooldown;
    public float ReloadTime;
}
public struct InitializeCameraTargetTag : IComponentData { }

public class PlayerAuthoring : MonoBehaviour
{
public int MagasineSize;
public float ReloadTime;

    public GameObject BulletPrefab;
    private class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PlayerTag>(entity);
            AddComponent<PlayerCommands>(entity);
            AddComponent<InitializeCameraTargetTag>(entity);
            AddComponent<ECSCameraTarget>(entity);
            AddComponent(entity, new PlayerData { BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic) });
            AddComponent(entity, new PlayerAttackData
            {
                CurrentBullets = authoring.MagasineSize,
                MagasineSize = authoring.MagasineSize,
                ReloadCooldown = authoring.ReloadTime,
                ReloadTime = authoring.ReloadTime
            });
        }
    }
}
