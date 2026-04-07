using System.Collections;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private EnemyAttackData attackData;
    [SerializeField] private Transform attackPoint;

    private LayerMask playerMask;

    public bool CanAttack {get; private set;} = true;

    void Awake()
    {
        playerMask = LayerMask.GetMask("Player");
    }

    public void Attack()
    {
        // There is only one player
        var playerColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackData.HitRadius, playerMask);
        foreach (var playerCollider in playerColliders)
        {
            Player player = playerCollider.GetComponent<Player>();
            player.ReceiveHit(transform, attackData.BaseDamage);
        }
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        CanAttack = false;
        yield return new WaitForSeconds(attackData.Cooldown);
        CanAttack = true;
    }

    public float DistanceToAttackPoint()
    {
        return Vector2.Distance(transform.position, attackPoint.position);
    }

    public float GetHitRadius()
    {
        return attackData.HitRadius;
    }

    void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackData.HitRadius);
    }
}
