using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarController : MonoBehaviour
{
    private Slider healthBar;

    private Health playerHealth;

    void Awake()
    {
        healthBar = GetComponent<Slider>();

        playerHealth = FindFirstObjectByType<Player>().GetComponent<Health>();

        SetMaxHealth(playerHealth.MaxHealth);
        SetHealth(playerHealth.MaxHealth);
    }

    void OnEnable()
    {
        playerHealth.MaxHealthChanged += SetMaxHealth;
        playerHealth.HealthChanged += SetHealth;
    }

    void OnDisable()
    {
        playerHealth.MaxHealthChanged -= SetMaxHealth;
        playerHealth.HealthChanged -= SetHealth;
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
