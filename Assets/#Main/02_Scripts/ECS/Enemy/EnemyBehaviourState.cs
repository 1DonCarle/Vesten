using Unity.Entities;
public enum EnemyBehaviour : byte
{
    Idle,
    Chase,
    Aim,
    Attack,
    Reload,
    Flee
}
public struct EnemyBehaviourState : IComponentData
{
    public EnemyBehaviour Value;
}
