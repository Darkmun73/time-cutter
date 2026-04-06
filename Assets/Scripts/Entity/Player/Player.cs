using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour, IMortal, IHittable
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
;
    }
    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void ReceiveHit(Transform from, float damage)
    {
        health.TakeDamage(damage);
        HitReceived?.Invoke(from);
    }
}
