using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(KnockbackController))]
public class Enemy : MonoBehaviour, IMortal, IHittable
{
    private Health health;
    private KnockbackController knockbackController;

    // events
    public event UnityAction<Transform> HitReceived;

    void Awake()
    {
        health = GetComponent<Health>();
        knockbackController = GetComponent<KnockbackController>();
    }

    void OnEnable()
    {
        HitReceived += knockbackController.Knockback;
        health.HealthDepleted += Die;
    }

    void OnDisable()
    {
        HitReceived -= knockbackController.Knockback;
        health.HealthDepleted -= Die;
    }

    // private void TryKnockback(Transform source)
    // {
    //     if (TryGetComponent(out KnockbackController knockbackController))
    //         knockbackController.Knockback(source);
    // }
    
    // private void HandlePlayerHit(GameObject hit)
    // {
    //     // MAYBE TODO: Если будет работать не точно, то мб поменять на IsTouching или подобное
    //     bool hitTouching = coll.Distance(hit.GetComponent<Collider2D>()).isOverlapped;
    //     if (hitTouching)
    //     {
    //         HandleHit(hit.transform.parent);
    //     }
    // }

    // public void SetVelocityX(float x)
    // {
    //     if (x > 0)
    //         MoveDirection = Direction.Right;
    //     else
    //         MoveDirection = Direction.Left;
    //     rigidBody.linearVelocityX = x;
    // }

    public void Die()
    {
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }

    public void ReceiveHit(Transform from, float damage) // TODO: переименовать в Receive и в PlayerCombat переименовать функцию
    {
        health.TakeDamage(damage);
        HitReceived?.Invoke(from);
    }
}
