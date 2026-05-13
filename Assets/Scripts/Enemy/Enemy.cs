using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour, IMortal, IHittable
{
    private Health health;
    private KnockbackController knockbackController;
    private Shield shield;

    private Transform lastHitSource;

    [SerializeField] private int currencyReward;

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
        if (lastHitSource.TryGetComponent<Player>(out var player))
        {
            var playerCurrency = player.GetComponent<Currency>();
            Debug.Assert(playerCurrency != null, "Enemy: Player must have currency!");
            playerCurrency.Amount += currencyReward;
        }
        Destroy(gameObject);
    }

    public void ReceiveHit(Transform from, float damage) // TODO: переименовать в Receive и в PlayerCombat переименовать функцию
    {
        if (shield != null && shield.TryBlock()) return;
        
        lastHitSource = from;
        health.TakeDamage(damage);
        HitReceived?.Invoke(from);
    }
}
