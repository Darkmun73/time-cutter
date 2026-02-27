using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerCollisionHandler : MonoBehaviour
{
    private Player player;
    private PlayerController playerController; // TODO: сделать ивенты в PlayerEvents, чтобы уменьшить coupling

    void Awake()
    {
        player = GetComponent<Player>();
        playerController = GetComponent<PlayerController>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            player.TakeDamage(enemy.Data.BaseDamage);
            playerController.Knockback(collision.transform.position);
        }
    }
}
