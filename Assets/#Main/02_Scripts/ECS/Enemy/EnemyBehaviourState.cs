using Unity.Entities;
public enum EnemyBehaviour : byte
{
    Idle,
    Chase,
    Aim,
    Attack,
    Reload,
    Flee,
    Dying
}
public struct EnemyBehaviourState : IComponentData
{
    public EnemyBehaviour Value;
}
