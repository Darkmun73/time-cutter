using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HitReceiver))]
public class DummyController : MonoBehaviour
{
    private Health health;
    private HitReceiver hitReceiver;

    void Awake()
    {
        health = GetComponent<Health>();
        hitReceiver = GetComponent<HitReceiver>();
    }

    void OnEnable()
    {
        hitReceiver.HitReceived += OnHitReceived;
        health.HealthDepleted += health.Reset;
    }

    void OnDisable()
    {
        hitReceiver.HitReceived -= OnHitReceived;
        health.HealthDepleted -= health.Reset;
    }

    private void OnHitReceived(HitInfo hitInfo)
    {
        health.TakeDamage(hitInfo.Damage);
    }
}
