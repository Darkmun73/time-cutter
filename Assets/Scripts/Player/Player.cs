using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour, IMortal
{
    private Health health;

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
}
