using UnityEngine;
using UnityEngine.VFX;

public class VFXManagedResources : MonoBehaviour
{
  public VisualEffect EnemyAttackGraph;

  public VisualEffect ImpactGraph;
public VisualEffect BloodSplatterGraph;
    public void Awake()
    {
    VFXReferences.MuzzleFlashGraph = EnemyAttackGraph;
    //VFXReferences.ImpactGraph = ImpactGraph;
       // VFXReferences.BloodSplatterGraph = BloodSplatterGraph;
    }
}
