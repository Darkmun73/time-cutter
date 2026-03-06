using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerCollisionHandler : MonoBehaviour
{
    private Player player;
    private KnockbackController knockbackController; // TODO: сделать ивенты в PlayerEvents, чтобы уменьшить coupling

    void Awake()
    {
        player = GetComponent<Player>();
        knockbackController = GetComponent<KnockbackController>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            player.TakeDamage(enemy.Data.BaseDamage);
            knockbackController.Knockback(collision.transform.position);
        }
    }
}
