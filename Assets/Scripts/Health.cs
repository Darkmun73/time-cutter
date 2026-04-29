using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public event UnityAction<float> HealthChanged;
    public event UnityAction<float> MaxHealthChanged;
    public event UnityAction HealthDepleted;

    [SerializeField] private HealthData data;
    
    private float currentHealth;

    private float maxHealthMultiplier = 1f;
    private float MaxHealthMultiplier
    {
        get => maxHealthMultiplier;
        set
        {
            maxHealthMultiplier = value;
            MaxHealthChanged?.Invoke(MaxHealth);
        }
    }

    public float MaxHealth => data.MaxHealth * MaxHealthMultiplier;

    private float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (value <= 0)
                currentHealth = 0;
            else if (value > MaxHealth)
                currentHealth = MaxHealth;
            else
                currentHealth = value;
            HealthChanged?.Invoke(currentHealth);

            if (currentHealth == 0)
                HealthDepleted?.Invoke();
        }
    }

    void Awake()
    {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    public void Heal(float value)
    {
        CurrentHealth += value;
    }

    public void IncreaseMaxHealthByFactor(float factor, bool restoreHealthToFull = true)
    {
        if (factor < 1f)
        {
            Debug.LogWarning("Factor must be >= 1!");
            return;
        }

        MaxHealthMultiplier *= factor;

        if (restoreHealthToFull)
            CurrentHealth = MaxHealth;
    }

    public void DecreaseMaxHealthByFactor(float factor)
    {
        if (factor < 1f)
        {
            Debug.LogWarning("Factor must be >= 1!");
            return;
        }

        MaxHealthMultiplier /= factor;
        CurrentHealth = currentHealth;
    }
}
