using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public event UnityAction<float> Damaged;
    public event UnityAction<float> HealthChanged;
    public event UnityAction<float> MaxHealthChanged;
    public event UnityAction HealthDepleted;

    [SerializeField] private HealthData data;
    

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

    private float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        private set
        {
            if (value <= 0)
                currentHealth = 0;
            else if (value > MaxHealth)
                currentHealth = MaxHealth;
            else
                currentHealth = value;
            HealthChanged?.Invoke(currentHealth);

            if (Mathf.Approximately(currentHealth, 0f))
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
        Damaged?.Invoke(damage);
    }

    public void Heal(float value)
    {
        CurrentHealth += value;
    }

    public void Reset()
    {
        CurrentHealth = MaxHealth;
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
