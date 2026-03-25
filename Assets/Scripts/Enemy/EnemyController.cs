using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    private Enemy enemy;
    private Rigidbody2D rigidBody;
    private KnockbackController knockbackController;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        rigidBody = GetComponent<Rigidbody2D>();
        knockbackController = GetComponent<KnockbackController>();
    }

    void Update()
    {
        if (knockbackController != null && knockbackController.IsKnockedBack)
            rigidBody.linearVelocity = Vector2.zero;
        
    }
}
