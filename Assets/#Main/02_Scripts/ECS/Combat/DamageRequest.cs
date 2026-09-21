using Unity.Entities;
using UnityEngine;

public struct DamageRequest : IBufferElementData
{
    public Entity Target;
    public Entity Source;
    public float Damage;
}
