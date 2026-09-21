using Unity.Entities;
using UnityEngine;
// This script will handle all the collision damage etc.

class CombatAuthoring : MonoBehaviour
{
    
}

class CombatAuthoringBaker : Baker<CombatAuthoring>
{
    public override void Bake(CombatAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        
            AddBuffer<DamageRequest>(entity);
    }
}
