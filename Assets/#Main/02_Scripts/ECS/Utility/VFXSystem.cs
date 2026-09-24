using Unity.Entities;
using UnityEngine;

public struct VFXEnemyAttackSingleton : IComponentData
{
    public VFXManager<VFXEnemyAttackRequest> Manager;
}
public struct VFXImpactSingleton : IComponentData
{
    public VFXManager<VFXImpactRequest> Manager;
}
partial struct VFXOnDeathSingleton : IComponentData
{
    public VFXManager<VFXOnDeathRequest> Manager;
}
partial struct VFXPlayerAttackSingleton : IComponentData
{
    public VFXManager<VFXPlayerAttackRequest> Manager;
}
partial struct VFXSystem : ISystem
{
    private int _spawnBatchId;
    private int _requestsCountId;
    private int _requestsBufferId;
    private int _datasBufferId;

    private VFXManager<VFXEnemyAttackRequest> _enemyAttackManager;
    private VFXManager<VFXImpactRequest> _impactManager;
    private VFXManager<VFXOnDeathRequest> _onDeathManager;
    private VFXManager<VFXPlayerAttackRequest> _playerAttackManager;


#if UNITY_ANDROID || UNITY_IOS
    public const int MuzzleFlashCapacity = 700;
        public const int ImpactCapacity = 700;
        public const int BloodSplatterCapacity = 700;

#else
    public const int MuzzleFlashCapacity = 1000;
    public const int ImpactCapacity = 1000;
    public const int BloodSplatterCapacity = 1000;

#endif

    public void OnCreate(ref SystemState state)
    {
        // Names to Ids
        _spawnBatchId = Shader.PropertyToID("SpawnBatch");
        _requestsCountId = Shader.PropertyToID("SpawnRequestsCount");
        _requestsBufferId = Shader.PropertyToID("SpawnRequestsBuffer");
        _datasBufferId = Shader.PropertyToID("DatasBuffer");

        _enemyAttackManager = new VFXManager<VFXEnemyAttackRequest>(
            MuzzleFlashCapacity,
            ref VFXReferences.MuzzleFlashRequestBuffer);

        _impactManager = new VFXManager<VFXImpactRequest>(
            ImpactCapacity,
            ref VFXReferences.ImpactRequestBuffer);

        _onDeathManager = new VFXManager<VFXOnDeathRequest>(
            BloodSplatterCapacity,
            ref VFXReferences.BloodSplatterRequestBuffer);
        
        _playerAttackManager = new VFXManager<VFXPlayerAttackRequest>(
            MuzzleFlashCapacity,
            ref VFXReferences.PlayerAttackRequestBuffer);

        state.EntityManager.AddComponentData(state.EntityManager.CreateEntity(), new VFXEnemyAttackSingleton
        {
            Manager = _enemyAttackManager,
        });
        state.EntityManager.AddComponentData(state.EntityManager.CreateEntity(), new VFXImpactSingleton
        {
            Manager = _impactManager,
        });
        state.EntityManager.AddComponentData(state.EntityManager.CreateEntity(), new VFXOnDeathSingleton
        {
            Manager = _onDeathManager,
        });
        state.EntityManager.AddComponentData(state.EntityManager.CreateEntity(), new VFXPlayerAttackSingleton
        {
            Manager = _playerAttackManager,
        });

    }

    public void OnUpdate(ref SystemState state)
    {
        SystemAPI.QueryBuilder().WithAll<VFXEnemyAttackSingleton>().Build().CompleteDependency();
        SystemAPI.QueryBuilder().WithAll<VFXImpactSingleton>().Build().CompleteDependency();
        SystemAPI.QueryBuilder().WithAll<VFXOnDeathSingleton>().Build().CompleteDependency();
        SystemAPI.QueryBuilder().WithAll<VFXPlayerAttackSingleton>().Build().CompleteDependency();
        float rateRatio = SystemAPI.Time.DeltaTime / Time.deltaTime;

        _enemyAttackManager.Update(
            VFXReferences.MuzzleFlashGraph,
            ref VFXReferences.MuzzleFlashRequestBuffer,
            rateRatio,
            _spawnBatchId,
            _requestsCountId,
            _requestsBufferId);

        _impactManager.Update(
            VFXReferences.ImpactGraph,
            ref VFXReferences.ImpactRequestBuffer,
            rateRatio,
            _spawnBatchId,
            _requestsCountId,
            _requestsBufferId);

        _onDeathManager.Update(
            VFXReferences.BloodSplatterGraph,
            ref VFXReferences.BloodSplatterRequestBuffer,
            rateRatio,
            _spawnBatchId,
            _requestsCountId,
            _requestsBufferId);

        _playerAttackManager.Update(
            VFXReferences.PlayerAttackGraph,
            ref VFXReferences.PlayerAttackRequestBuffer,
            rateRatio,
            _spawnBatchId,
            _requestsCountId,
            _requestsBufferId);
    }

    public void OnDestroy(ref SystemState state)
    {
        _enemyAttackManager.Dispose(ref VFXReferences.MuzzleFlashRequestBuffer);
        _impactManager.Dispose(ref VFXReferences.ImpactRequestBuffer);
        _onDeathManager.Dispose(ref VFXReferences.BloodSplatterRequestBuffer);
        _playerAttackManager.Dispose(ref VFXReferences.PlayerAttackRequestBuffer);
    }
}
