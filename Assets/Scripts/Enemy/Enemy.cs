using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour, IMortal, IHittable
{
    private Health health;
    private KnockbackController knockbackController;
    private Shield shield;
    

    // events
    public event UnityAction<Transform> HitReceived;

    void Awake()
    {
        health = GetComponent<Health>();
        knockbackController = GetComponent<KnockbackController>();
        shield = GetComponent<Shield>();
    }

    void OnEnable()
    {
        if (knockbackController != null)
            HitReceived += knockbackController.Knockback;
        health.HealthDepleted += Die;
    }

    void OnDisable()
    {
        if (knockbackController != null)
            HitReceived -= knockbackController.Knockback;
        health.HealthDepleted -= Die;
    }

    public void Die()
    {
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }

    public void ReceiveHit(Transform from, float damage) // TODO: переименовать в Receive и в PlayerCombat переименовать функцию
    {
        if (shield != null && shield.TryBlock()) return;
        
        health.TakeDamage(damage);
        HitReceived?.Invoke(from);
    }
}
