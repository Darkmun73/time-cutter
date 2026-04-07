using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public event UnityAction<float> HealthChanged;
    public event UnityAction HealthDepleted;

    [SerializeField] private HealthData data;
    
    private float currentHealth;
    private float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (value <= 0)
                currentHealth = 0;
            else if (value > data.MaxHealth)
                currentHealth = data.MaxHealth;
            else
                currentHealth = value;
            HealthChanged?.Invoke(currentHealth);

            if (currentHealth == 0)
                HealthDepleted?.Invoke();
        }
    }

    void Awake()
    {
        currentHealth = data.MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    public float GetMaxHealth()
    {
        return data.MaxHealth;
    }
    
}