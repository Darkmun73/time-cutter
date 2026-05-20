using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour, IMortal, IHittable
{
    private Health health;

    // events
    public event UnityAction<Transform> HitReceived;

    void Awake()
    {
        health = GetComponent<Health>();
    }

    void OnEnable()
    {
        health.HealthDepleted += Die;
    }

    void OnDisable()
    {
        health.HealthDepleted -= Die;
;
    }
    public void Die()
    {
        //throw new System.NotImplementedException();
    }

    public void ReceiveHit(Transform from, float damage)
    {
        health.TakeDamage(damage);
        HitReceived?.Invoke(from);
    }
}
