using UnityEngine;

[RequireComponent(typeof(HitReceiver))]
public class HitBloodSplasher : MonoBehaviour, IHitInfoVisitor
{
    public void Visit(PlayerHitInfo hitInfo)
    {
        Debug.Log("blood splash from player");
    }

    public void Visit(EnemyHitInfo hitInfo)
    {
        Debug.Log("blood splash from enemy");
    }
}