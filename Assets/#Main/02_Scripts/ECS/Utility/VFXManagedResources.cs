using UnityEngine;
using UnityEngine.VFX;

public class VFXManagedResources : MonoBehaviour
{
  public VisualEffect EnemyAttackGraph;

  public VisualEffect ImpactGraph;
public VisualEffect BloodSplatterGraph;
public VisualEffect PlayerAttackGraph;
    public void Awake()
    {
    VFXReferences.MuzzleFlashGraph = EnemyAttackGraph;
    VFXReferences.PlayerAttackGraph = PlayerAttackGraph;
    //VFXReferences.ImpactGraph = ImpactGraph;
       // VFXReferences.BloodSplatterGraph = BloodSplatterGraph;
    }
}
