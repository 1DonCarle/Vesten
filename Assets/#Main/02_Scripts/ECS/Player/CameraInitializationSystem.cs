using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using static PlayerAuthoring;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct CameraInitializationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InitializeCameraTargetTag>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if (CameraTargetSingleton.Instance == null) return;

        var cameraTransform = CameraTargetSingleton.Instance.transform;

        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (cameraTarget, entity) in SystemAPI.Query<RefRW<ECSCameraTarget>>().
            WithAll<InitializeCameraTargetTag, PlayerTag>().
            WithEntityAccess())
        {
            cameraTarget.ValueRW.CameraTransform = cameraTransform;
            ecb.RemoveComponent<InitializeCameraTargetTag>(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}
[UpdateAfter(typeof(TransformSystemGroup))]
public partial struct MoveCameraSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, cameraTarget) in SystemAPI.Query<LocalToWorld, ECSCameraTarget>().WithAll<PlayerTag>().
            WithNone<InitializeCameraTargetTag>())
        {
            cameraTarget.CameraTransform.Value.position = transform.Position;
        }
    }
}
