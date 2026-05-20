using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarController : MonoBehaviour
{
    private Slider healthBar;

    [SerializeField] private Health health;

    void Awake()
    {
        healthBar = GetComponent<Slider>();

        SetMaxHealth(health.MaxHealth);
        SetHealth(health.MaxHealth);
    }

    void OnEnable()
    {
        health.MaxHealthChanged += SetMaxHealth;
        health.HealthChanged += SetHealth;
    }

    void OnDisable()
    {
        health.MaxHealthChanged -= SetMaxHealth;
        health.HealthChanged -= SetHealth;
    }

    void SetMaxHealth(float value)
    {
        healthBar.maxValue = value;
    }

    void SetHealth(float value)
    {
        healthBar.value = value;
    }
}
