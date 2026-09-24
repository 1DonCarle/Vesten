using UnityEngine;
using UnityEngine.VFX;

public class VFXManagedResources : MonoBehaviour
{
  public VisualEffect EnemyAttackGraph;

  public VisualEffect ImpactGraph;
  public VisualEffect BloodSplatterGraph;
  public VisualEffect PlayerAttackGraph;

  private GraphicsBuffer destroyBulletBuffer;

  private const int MaxDestroyRequests = 128;
  public void Awake()
  {
    VFXReferences.MuzzleFlashGraph = EnemyAttackGraph;
    VFXReferences.PlayerAttackGraph = PlayerAttackGraph;
    //VFXReferences.ImpactGraph = ImpactGraph;
    // VFXReferences.BloodSplatterGraph = BloodSplatterGraph;
    destroyBulletBuffer = new GraphicsBuffer(
                GraphicsBuffer.Target.Structured,
                MaxDestroyRequests,
                sizeof(uint)
            );
  }

  private void OnDestroy()
  {
    destroyBulletBuffer?.Release();
  }
  public void SetDestroyRequests(uint[] ids, int count)
{
    if (count == 0)
    {
        PlayerAttackGraph.SetInt("DestroyRequestCount", 0);
        return;
    }

    destroyBulletBuffer.SetData(ids, 0, 0, count);

    PlayerAttackGraph.SetGraphicsBuffer(
        "DestroyRequests",
        destroyBulletBuffer
    );

    PlayerAttackGraph.SetInt(
        "DestroyRequestCount",
        count
    );
}
}