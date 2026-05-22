using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HitReceiver))]
public class Enemy : MonoBehaviour, IMortal
{
    private Health health;
    private HitReceiver hitReceiver;

    [SerializeField] private int currencyReward;

    void Awake()
    {
        health = GetComponent<Health>();
        hitReceiver = GetComponent<HitReceiver>();
    }

    void OnEnable()
    {
        health.HealthDepleted += Die;
    }

    void OnDisable()
    {
        health.HealthDepleted -= Die;
    }

    public void Die()
    {
        Debug.Log("Enemy died");
        if (hitReceiver.LastHitSource != null && hitReceiver.LastHitSource.TryGetComponent<Player>(out var player))
        {
            var playerCurrency = player.GetComponent<Currency>();
            Debug.Assert(playerCurrency != null, "Enemy: Player must have currency!");
            playerCurrency.Amount += currencyReward;
        }
        Destroy(gameObject);
    }
}
