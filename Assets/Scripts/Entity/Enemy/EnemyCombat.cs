using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private Enemy enemy;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 2f;

    private LayerMask playerMask;

    void Awake()
    {
        enemy = GetComponent<Enemy>();

        playerMask = LayerMask.GetMask("Player");
    }

    public void Attack()
    {
        // There is only one player
        var playerColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerMask);
        foreach (var playerCollider in playerColliders)
        {
            Player player = playerCollider.GetComponent<Player>();
            player.HandleHit(transform, enemy.Data.BaseDamage);
        }
    }

    void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
