using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyCollisionHandler : MonoBehaviour
{
    private KnockbackController knockbackController; // TODO: сделать ивенты, чтобы уменьшить coupling

    void Awake()
    {
        knockbackController = GetComponent<KnockbackController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hit"))
        {
            Debug.Log("in");
            knockbackController?.Knockback(collision.transform.parent.position);
        }
    }
}
