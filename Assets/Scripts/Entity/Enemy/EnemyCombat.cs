using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private EnemyAttackData attackData;
    [SerializeField] private Transform attackPoint;

    private LayerMask playerMask;

    void Awake()
    {
        playerMask = LayerMask.GetMask("Player");
    }

    public void Attack()
    {
        // There is only one player
        var playerColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackData.Radius, playerMask);
        foreach (var playerCollider in playerColliders)
        {
            Player player = playerCollider.GetComponent<Player>();
            player.ReceiveHit(transform, attackData.BaseDamage);
        }
    }

    void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackData.Radius);
    }
}
